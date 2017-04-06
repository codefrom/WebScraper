//-----------------------------------------------------------------------
// <copyright file="IExtractor.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Interfaces.TaskElements
{
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// Interface for extractor/transformer task element
    /// </summary>
    /// <typeparam name="T">Type of payload that will be returned by this element</typeparam>
    /// <typeparam name="K">Type of payload that element work with </typeparam>
    public interface IExtractor<T, K> : ITaskElement
        where T : IPayload
        where K : IPayload
    {
        /// <summary>
        /// Extracts/transforms payloads from another payload
        /// </summary>
        /// <param name="payload">Payload of type <typeparamref name="K"/> that will be processed</param>
        /// <returns>Enumeration of objects of type <typeparamref name="T"/></returns>
        IEnumerable<T> ExtractFrom(K payload);
    }
}
