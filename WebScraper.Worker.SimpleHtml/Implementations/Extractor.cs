//-----------------------------------------------------------------------
// <copyright file="Extractor.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System.Collections.Generic;
    using Common;
    using CsQuery;
    using Interfaces.TaskElements;

    /// <summary>
    /// Extracts data from html, using CSS selector
    /// </summary>
    public class Extractor : IExtractor<HtmlPayload>
    {
        /// <summary>
        /// Gets or sets CSS selector string
        /// </summary>
        public string CssSelector { get; set; }

        /// <summary>
        /// Extract data from html
        /// </summary>
        /// <param name="payload">HtmlPayload from which will be extracting</param>
        /// <returns>Returns list of HtmlPayload</returns>
        public IEnumerable<HtmlPayload> ExtractFrom(IPayload payload)
        {
            var html = payload as HtmlPayload;
            var elements = new List<HtmlPayload>();
            foreach (var element in html.Content[this.CssSelector])
            {
                elements.Add(new HtmlPayload() { Content = new CQ(element) });
            }

            return elements;
        }
    }
}