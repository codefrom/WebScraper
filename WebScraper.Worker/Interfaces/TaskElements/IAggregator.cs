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
    public interface IAggregator<out T> : ITaskElement
        where T : IPayload
    {
        /// <summary>
        /// Aggregate input payloads into one object
        /// </summary>
        /// <param name="payloads">Payloads of type IPayload that will be aggregated</param>
        /// <returns>One object of type <typeparamref name="T"/></returns>
        T Aggregate(IEnumerable<IPayload> payloads);
    }
}
