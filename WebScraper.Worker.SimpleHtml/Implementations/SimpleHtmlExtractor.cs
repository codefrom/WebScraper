namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System.Collections.Generic;
    using Common;
    using CsQuery;
    using Interfaces.TaskElements;

    public class SimpleHtmlExtractor : IExtractor<SimpleHtmlPayload>
    {
        /// <summary>
        /// Gets or sets css selector string
        /// </summary>
        public string CssSelector { get; set; }

        public IEnumerable<SimpleHtmlPayload> ExtractFrom(IPayload payload)
        {
            var html = payload as SimpleHtmlPayload;
            var elements = new List<SimpleHtmlPayload>();
            foreach (var element in html.Content[CssSelector])
            {
                elements.Add(new SimpleHtmlPayload() { Content = new CQ(element) } );
            }
            return elements;
        }
    }
}
