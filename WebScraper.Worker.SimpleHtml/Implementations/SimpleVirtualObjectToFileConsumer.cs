namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System;
    using Common;
    using NLog;
    using Interfaces.TaskElements;
    using System.IO;
    using SimpleHtml.Implementations;

    public class SimpleVirtualObjectToFileConsumer : IConsumer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string FilePath { get; set; }

        public void Consume(IPayload payload)
        {
            var vo = payload as VirtualObjectPayload;
            if (vo != null)
            {
                using (var sw = File.AppendText(FilePath))
                {
                    sw.Write(vo);
                }
            }
            else
            {
                throw new ArgumentNullException("payload");
            }
        }
    }
}
