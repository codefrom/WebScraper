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
    using System.Net;
    using Interfaces.TaskElements;
    
    /// <summary>
    /// Simple http provider
    /// </summary>
    public class SimpleHtmlProvider : IProvider<HtmlPayload>
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
                    Content = 
                        WebRequest.Create(this.Address)
                            .GetResponse()
                            .GetResponseStream()
                }
            };
        }
    }
}
