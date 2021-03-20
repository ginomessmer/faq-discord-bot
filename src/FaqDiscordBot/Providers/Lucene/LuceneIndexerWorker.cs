using FaqDiscordBot.Abstractions;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FaqDiscordBot.Providers.Lucene
{
    public class LuceneIndexerWorker : BackgroundService
    {
        private readonly IndexWriter _indexWriter;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<LuceneIndexerWorker> _logger;

        public LuceneIndexerWorker(IndexWriter indexWriter, IHostEnvironment environment,
            ILogger<LuceneIndexerWorker> logger)
        {
            _indexWriter = indexWriter;
            _environment = environment;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var contents = _environment.ContentRootFileProvider.GetDirectoryContents("data/sources");
            if (!contents.Exists)
                return;

            foreach (var fileInfo in contents)
            {
                var text = await File.ReadAllTextAsync(fileInfo.PhysicalPath, stoppingToken);

                var doc = new Document
                {
                    new StringField("Name", fileInfo.Name, Field.Store.YES),
                    new TextField(nameof(IAnswer.Answer), text, Field.Store.YES)
                };

                _indexWriter.AddDocument(doc);
            }

            _logger.LogInformation("Indexed {Count} documents", contents.Count());
            _indexWriter.Flush(triggerMerge: true, applyAllDeletes: true);
            _indexWriter.Commit();
        }
    }
}