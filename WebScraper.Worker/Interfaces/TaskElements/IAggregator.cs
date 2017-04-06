//-----------------------------------------------------------------------
// <copyright file="IAggregator.cs" company="CodeFrom">
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
    /// Interface for aggregator node in task
    /// </summary>
    /// <typeparam name="T">Type of payload that aggregator returns</typeparam>
    /// <typeparam name="K">Type of payload that aggregator can accept on input</typeparam>
    public interface IAggregator<T, K> : ITaskElement
        where T : IPayload
        where K : IPayload
    {
        /// <summary>
        /// Aggregate input payloads into one object
        /// </summary>
        /// <param name="payloads">Payloads of type <typeparamref name="K"/> that will be aggregated</param>
        /// <returns>One object of type <typeparamref name="T"/></returns>
        T Aggregate(IEnumerable<K> payloads);
    }
}
