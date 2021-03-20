using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaqDiscordBot.Abstractions;
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace FaqDiscordBot.Providers.Local
{
    public class LuceneFaqService : IFaqService
    {
        private readonly IndexSearcher _indexSearcher;

        public LuceneFaqService(IndexSearcher indexSearcher)
        {
            _indexSearcher = indexSearcher;
        }

        /// <inheritdoc />
        public Task<IEnumerable<IAnswer>> AskAsync(string question) =>
            Task.Factory.StartNew(() => Ask(question));

        private IEnumerable<IAnswer> Ask(string question)
        {
            var terms = question.Split(' ').Select(x => new Term(nameof(IAnswer.Answer), x));
            var query = new BooleanQuery();
            foreach (var term in terms)
            {
                query.Add(new TermQuery(term), Occur.SHOULD);
            }

            var hits = _indexSearcher.Search(query, 5);

            var documents = hits.ScoreDocs.Select(x =>
            {
                var doc = _indexSearcher.Doc(x.Doc);
                return new LuceneAnswer
                {
                    Answer = doc.Get(nameof(IAnswer.Answer)),
                    Score = x.Score
                };
            });

            return documents;
        }
    }
}
