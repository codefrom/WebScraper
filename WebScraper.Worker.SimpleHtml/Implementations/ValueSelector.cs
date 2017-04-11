//-----------------------------------------------------------------------
// <copyright file="ValueSelector.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System.Collections.Generic;
    using CsQuery;

    /// <summary>
    /// Class represents selector for extracting value from html page
    /// </summary>
    public class ValueSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueSelector" /> class with default parameters
        /// </summary>
        public ValueSelector()
        {
            this.CssSelector = null;
            this.GetAttribute = null;
            this.GetText = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueSelector" /> class with named parameters
        /// </summary>
        /// <param name="css">Value of <seealso cref="CssSelector"/>, default is empty</param>
        /// <param name="attribute">Value of GetAttribute, default is empty</param>
        /// <param name="getText">Value of GetText, default is true</param>
        public ValueSelector(string css = "", string attribute = "", bool getText = true)
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
        public string Select(CQ inputContent)
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

        /// <summary>
        /// Applies current selector to html, to get resulting string value
        /// </summary>
        /// <param name="inputContent">Input html to be processed</param>
        /// <returns>Enumeration of string value of selected attribute or element</returns>
        public IEnumerable<string> SelectMany(CQ inputContent)
        {
            var contents = inputContent;
            if (!string.IsNullOrEmpty(this.CssSelector))
            {
                contents = contents.Find(this.CssSelector);
            }

            foreach (var content in contents)
            {
                if (this.GetText)
                {
                    yield return content.InnerText;
                }
                else
                {
                    yield return content.GetAttribute(this.GetAttribute);
                }
            }

            yield break;
        }
    }
}
