//-----------------------------------------------------------------------
// <copyright file="SimpleHtmlProvider.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System.Collections.Generic;
    using Interfaces.TaskElements;
    using CsQuery;

    /// <summary>
    /// Simple http provider
    /// </summary>
    public class SimpleHtmlProvider : IProvider<SimpleHtmlPayload>
    {
        /// <summary>
        /// Gets or sets address to be loaded
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Provide payload from link
        /// </summary>
        /// <returns>Returns payload of loaded page</returns>
        public IEnumerable<SimpleHtmlPayload> Provide()
        {
            return new List<SimpleHtmlPayload>()
            {
                new SimpleHtmlPayload()
                {
                    Content = CQ.CreateFromUrl(this.Address)
                }
            };
        }
    }
}
