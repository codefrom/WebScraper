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
    using System.Collections.Generic;
    using Common;
    using CsQuery;
    using Interfaces.TaskElements;

    /// <summary>
    /// Class for transformation from html to virtual object
    /// </summary>
    public class HtmlToVirtualObjectTransformer : ITransformer<VirtualObjectPayload>
    {
        /// <summary>
        /// Gets or sets dictionary to be used for mapping
        ///   keys - field names in VirtualObject
        ///   values - selector to be applied to payload
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
            foreach (var map in this.Mapping)
            {
                res.SetProperty(map.Key, map.Value.ApplyTo(html.Content));
            }

            return res;
        }

        /// <summary>
        /// Class represents selector for mapping
        /// </summary>
        public class Selector
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Selector" /> class with default parameters
            /// </summary>
            public Selector()
            {
                this.CssSelector = null;
                this.GetAttribute = null;
                this.GetText = true;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Selector" /> class with named parameters
            /// </summary>
            /// <param name="css">Value of <seealso cref="CssSelector"/>, default is empty</param>
            /// <param name="attribute">Value of GetAttribute, default is empty</param>
            /// <param name="getText">Value of GetText, default is true</param>
            public Selector(string css = "", string attribute = "", bool getText = true)
            {
                this.CssSelector = css;
                this.GetAttribute = attribute;
                this.GetText = string.IsNullOrEmpty(attribute);
            }

            /// <summary>
            /// Gets or sets CSS selector to use before get value
            /// </summary>
            public string CssSelector { get; set; }

            /// <summary>
            /// Gets or sets attribute name value of which to get
            /// </summary>
            public string GetAttribute { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether to get text from element (not attribute)
            /// </summary>
            public bool GetText { get; set; }

            /// <summary>
            /// Applies current selector to html, to get resulting string value
            /// </summary>
            /// <param name="inputContent">Input html to be processed</param>
            /// <returns>String value of selected attribute or element</returns>
            public string ApplyTo(CQ inputContent)
            {
                var content = inputContent;
                if (!string.IsNullOrEmpty(this.CssSelector))
                {
                    content = content.Find(this.CssSelector);
                }

                if (this.GetText)
                {
                    return content.Text();
                }
                else
                {
                    return content.Attr(this.GetAttribute);
                }
            }
        }
    }
}
