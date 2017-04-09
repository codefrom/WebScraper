namespace WebScraper.DebugConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodeFrom.WebScraper.Common;
    using CodeFrom.WebScraper.Worker.Implementations;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using CodeFrom.WebScraper.Worker.SimpleHtml.Implementations;

    public class Program
    {
        public static void Main(string[] args)
        {
            TaskParallelProcessor a = new TaskParallelProcessor() { DegreeOfParallelism = 10 };
            var l = new List<ITaskElement>();
            l.Add(new Provider() { Address = "https://bitskins.com" });
            l.Add(new Extractor() { CssSelector = "div.item-solo" });
            l.Add(new HtmlToVirtualObjectTransformer() { Mapping = new Dictionary<string, HtmlToVirtualObjectTransformer.Selector>()
            {
                { "price" , new HtmlToVirtualObjectTransformer.Selector(css: "span.item-price") },
                { "name" , new HtmlToVirtualObjectTransformer.Selector(css: "div.item-title") }
            }
            });
            l.Add(new VirtualObjectToFileConsumer() { FilePath = "test.out.txt" });
            a.TaskElements = l;
            a.DoTask();
        }
    }
}
