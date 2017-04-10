namespace WebScraper.DebugConsole
{
    using System.Collections.Generic;
    using CodeFrom.WebScraper.Worker.Implementations;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using CodeFrom.WebScraper.Worker.SimpleHtml.Implementations;

    public class Program
    {
        public static void Main(string[] args)
        {
            TaskParallelProcessor a = new TaskParallelProcessor() { DegreeOfParallelism = 10 };
            var l = new List<ITaskElement>();
            l.Add(new Provider() { Address = "https://google.com/?q=WebScraper" });
            l.Add(new Extractor() { CssSelector = "h3.r" });
            l.Add(new HtmlToVirtualObjectTransformer() { Mapping = new Dictionary<string, HtmlToVirtualObjectTransformer.Selector>()
            {
                { "text" , new HtmlToVirtualObjectTransformer.Selector(css: "a") },
                { "href" , new HtmlToVirtualObjectTransformer.Selector(css: "a", attribute: "href") },
                { "target" , new HtmlToVirtualObjectTransformer.Selector(css: "a", attribute: "target") }
            }
            });
            l.Add(new VirtualObjectToFileConsumer() { FilePath = "test.out.txt" });
            a.TaskElements = l;
            a.DoTask();
        }
    }
}
