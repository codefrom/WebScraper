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
    using NLog;

    /// <summary>
    /// Class for transformation from html to virtual object
    /// </summary>
    public class HtmlToVirtualObjectTransformer : ITransformer<VirtualObjectPayload>
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets dictionary to be used for mapping
        ///   keys - field names in VirtualObject
        ///   values - selector to be applied to payload
        /// </summary>
        public Dictionary<string, ValueSelector> Mapping { get; set; }

        /// <summary>
        /// Transforms payload from HtmlPayload to VirtualObjectPayload
        /// </summary>
        /// <param name="payload">Payload of type HtmlPayload</param>
        /// <returns>Returns object of type VirtualObjectPayload</returns>
        public VirtualObjectPayload Transform(IPayload payload)
        {
            logger.Debug($"Start transforming payload");
            logger.Trace($"Payload : {payload}");
            var html = payload as HtmlPayload;
            if (html == null)
            {
                logger.Error($"Got wrong payload type : {payload.GetType()}, expecting HtmlPayload");
                throw new ArgumentNullException("payload");
            }

            var res = new VirtualObjectPayload();
            foreach (var map in this.Mapping)
            {
                res.SetProperty(map.Key, map.Value.Select(html.Content));
            }

            logger.Debug($"Transformed payload");
            return res;
        }
    }
}
