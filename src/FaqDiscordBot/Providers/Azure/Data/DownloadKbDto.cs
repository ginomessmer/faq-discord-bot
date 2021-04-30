using System.Collections.Generic;

namespace FaqDiscordBot.Providers.Azure.Data
{
    public class DownloadKbDto
    {
        public IEnumerable<QnaDto> QnaDocuments { get; set; }
    }
}