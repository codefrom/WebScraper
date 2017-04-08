
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System;
    using Common;
    using Interfaces.TaskElements;
    using Worker.Implementations;
    using System.Collections.Generic;
    using CsQuery;

    public class SimpleHtmlToVirtualObjectTransformer : ITransformer<VirtualObjectPayload>
    {
        public class Selector
        {
            public string CssSelector { get; set; }
            public string GetAttribute { get; set; }
            public bool GetText { get; set; }

            public Selector()
            {
                this.CssSelector = null;
                this.GetAttribute = null;
                this.GetText = true;
            }

            public Selector(string css = "", string attribute = "", bool getText = true)
            {
                this.CssSelector = css;
                this.GetAttribute = attribute;
                this.GetText = string.IsNullOrEmpty(attribute);
            }

            public string ApplyTo(CQ inputContent)
            {
                var content = inputContent;
                if (!string.IsNullOrEmpty(CssSelector))
                {
                    content = content.Find(CssSelector);
                }

                if (this.GetText)
                {
                    return content.Text();
                }
                else
                {
                    return content.Attr(GetAttribute);
                }
            }
        }

        public Dictionary<string, Selector> Mapping = new Dictionary<string, Selector>();

        public VirtualObjectPayload Transform(IPayload payload)
        {
            var html = payload as SimpleHtmlPayload;
            if (html == null)
            {
                throw new ArgumentNullException("payload");
            }

            var res = new VirtualObjectPayload();
            foreach (var map in Mapping)
            {
                res.SetProperty(map.Key, map.Value.ApplyTo(html.Content));
            }
            return res;
        }
    }

}
