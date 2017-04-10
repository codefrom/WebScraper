//-----------------------------------------------------------------------
// <copyright file="Provider.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System.Collections.Generic;
    using CsQuery;
    using Interfaces.TaskElements;

    /// <summary>
    /// Simple http provider
    /// </summary>
    public class Provider : IProvider<HtmlPayload>
    {
        /// <summary>
        /// Gets or sets address to be loaded
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Provide payload from link
        /// </summary>
        /// <returns>Returns payload of loaded page</returns>
        public IEnumerable<HtmlPayload> Provide()
        {
            return new List<HtmlPayload>()
            {
                new HtmlPayload()
                {
                    Content = CQ.CreateFromUrl(this.Address)
                }
            };
        }
    }
}
