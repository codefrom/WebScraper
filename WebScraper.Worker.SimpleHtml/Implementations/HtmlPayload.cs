//-----------------------------------------------------------------------
// <copyright file="HtmlPayload.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System;
    using Common;
    using CsQuery;

    /// <summary>
    /// Html payload implementation
    /// </summary>
    public class HtmlPayload : IHtmlPayload
    {
        /// <summary>
        /// Gets or sets url at which page is located
        /// </summary>
        public Uri Address { get; set; }

        /// <summary>
        /// Gets or sets html content
        /// </summary>
        public CQ Content { get; set; }
    }
}
