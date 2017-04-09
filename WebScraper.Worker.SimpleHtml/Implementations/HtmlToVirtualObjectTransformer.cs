//-----------------------------------------------------------------------
// <copyright file="HtmlToVirtualObjectTransformer.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System;
    using Common;
    using Interfaces.TaskElements;
    using System.Collections.Generic;
    using CsQuery;

    /// <summary>
    /// Class for transformation from html to virtual object
    /// </summary>
    public class HtmlToVirtualObjectTransformer : ITransformer<VirtualObjectPayload>
    {
        /// <summary>
        /// Class represents sellector for mapping
        /// </summary>
        public class Selector
        {
            /// <summary>
            /// Gets or sets css selector to use before get value
            /// </summary>
            public string CssSelector { get; set; }

            /// <summary>
            /// Gets or sets attribute name value of which to get
            /// </summary>
            public string GetAttribute { get; set; }

            /// <summary>
            /// Gets or sets if need to get text from element (not attribute)
            /// </summary>
            public bool GetText { get; set; }

            /// <summary>
            /// Basic constructor
            /// </summary>
            public Selector()
            {
                this.CssSelector = null;
                this.GetAttribute = null;
                this.GetText = true;
            }

            /// <summary>
            /// Constructor with named params
            /// </summary>
            /// <param name="css">Value of CssSelector, default is empty</param>
            /// <param name="attribute">Value of GetAttribute, default is empty</param>
            /// <param name="getText">Value of GetText, default is true</param>
            public Selector(string css = "", string attribute = "", bool getText = true)
            {
                this.CssSelector = css;
                this.GetAttribute = attribute;
                this.GetText = string.IsNullOrEmpty(attribute);
            }

            /// <summary>
            /// Applies current selector to html, to get resulting string value
            /// </summary>
            /// <param name="inputContent">Input html to be processed</param>
            /// <returns>String value of selected attribute or element</returns>
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

        /// <summary>
        /// Gets or sets dictionary to be used for mapping
        ///   keys - field names in VirtualObject
        ///   values - selector to be applied to paylad
        /// </summary>
        public Dictionary<string, Selector> Mapping { get; set; }

        /// <summary>
        /// Transforms payload from HtmlPayload to VirtualObjectPayload
        /// </summary>
        /// <param name="payload">Payload of type HtmlPayload</param>
        /// <returns>Returns object of type VirtualObjectPayload</returns>
        public VirtualObjectPayload Transform(IPayload payload)
        {
            var html = payload as HtmlPayload;
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
