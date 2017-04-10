﻿//-----------------------------------------------------------------------
// <copyright file="VirtualObjectToFileConsumer.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System;
    using System.IO;
    using Common;
    using Interfaces.TaskElements;

    /// <summary>
    /// Consumes VirtualObject to file
    /// </summary>
    public class VirtualObjectToFileConsumer : IConsumer
    {
        /// <summary>
        /// Gets or sets path to file payload to be written in
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Consumes VirtualObjectPayload and writes it to file at FilePath
        /// </summary>
        /// <param name="payload">Payload to be consumed</param>
        public void Consume(IPayload payload)
        {
            var vo = payload as VirtualObjectPayload;
            if (vo != null)
            {
                using (var sw = File.AppendText(this.FilePath))
                {
                    sw.Write(vo);
                }
            }
            else
            {
                throw new ArgumentNullException("payload");
            }
        }
    }
}
