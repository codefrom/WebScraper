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
    public interface ISplitter<out T> : ITaskElement
        where T : IPayload
    {
        /// <summary>
        /// Split incoming payload to enumeration of payloads
        /// </summary>
        /// <param name="payload">Payload of type IPayload to be processed</param>
        /// <returns>Enumeration of payloads of type <typeparamref name="T"/></returns>
        IEnumerable<T> Split(IPayload payload);
    }
}
