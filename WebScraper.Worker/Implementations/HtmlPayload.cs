﻿//-----------------------------------------------------------------------
// <copyright file="HtmlPayload.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System.IO;
    using Common;

    /// <summary>
    /// Html payload implementation
    /// </summary>
    public class HtmlPayload : IHtmlPayload
    {
        /// <summary>
        /// Gets or sets html content
        /// </summary>
        public Stream Content { get; set; }
    }
}
