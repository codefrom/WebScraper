namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System;
    using Common;
    using NLog;
    using Interfaces.TaskElements;
    using System.IO;

    public class SimpleHtmlToFileConsumer : IConsumer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string FilePath { get; set; }

        public void Consume(IPayload payload)
        {
            var html = payload as SimpleHtmlPayload;
            if (html != null)
            {
                using (var sw = File.AppendText(FilePath))
                {
                    sw.WriteLine(html.Content.RenderSelection());
                }
            }
            else
            {
                throw new ArgumentNullException("payload");
            }
        }
    }
}
