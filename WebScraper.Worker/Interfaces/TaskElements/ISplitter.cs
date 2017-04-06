//-----------------------------------------------------------------------
// <copyright file="ISplitter.cs" company="CodeFrom">
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
    /// Interface of splitter that will take one payload and provide enumeration of other payloads
    /// </summary>
    /// <typeparam name="T">Type of payloads to return</typeparam>
    /// <typeparam name="K">Type of payload to process</typeparam>
    public interface ISplitter<T, K> : ITaskElement
        where T : IPayload
        where K : IPayload
    {
        /// <summary>
        /// Split incoming payload to enumeration of payloads
        /// </summary>
        /// <param name="payload">Payload of type <typeparamref name="K"/> to be processed</param>
        /// <returns>Enumeration of payloads of type <typeparamref name="T"/></returns>
        IEnumerable<T> Split(K payload);
    }
}
