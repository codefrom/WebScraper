namespace WebScraper.DebugConsole
{
    using System.Collections.Generic;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using CodeFrom.WebScraper.Worker.SimpleHtml.Implementations;
    using Worker.Akka.Implementations;

    public class Program
    {
        public static void Main(string[] args)
        {
            AkkaTaskProcessor a = new AkkaTaskProcessor(name: "example")
            {
                TaskElements = new List<ITaskElement>()
                {
                    new Provider() { Address = "https://duckduckgo.com/html/?q=WebScraper",
                                     MaxDepth = 1,
                                     NextUrlSelector = new ValueSelector(css: "a", attribute: "href") },
                    //new Extractor() { CssSelector = "div.result__body" },
                    new Extractor() { CssSelector = "a" },
                    new HtmlToVirtualObjectTransformer() { Mapping = new Dictionary<string, ValueSelector>()
                    {
                        { "title" , new ValueSelector() },
                        { "href" , new ValueSelector(attribute: "href") },
                        { "target" , new ValueSelector(attribute: "target") }
/*                        { "title" , new ValueSelector(css: ".result__a") },
                        { "description" , new ValueSelector(css: ".result__snippet") },
                        { "href" , new ValueSelector(css: "a.result__a", attribute: "href") },
                        { "target" , new ValueSelector(css: "a.result__a", attribute: "target") }*/
                    }},
                    new VirtualObjectToFileConsumer() { FilePath = "test.out.txt" }
                }
            };
            a.DoTask();
        }
    }
}
