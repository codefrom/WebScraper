//-----------------------------------------------------------------------
// <copyright file="IProvider.cs" company="CodeFrom">
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
    /// Interface for provider. It's main function is to get some payload/payloads for processing (how ever it pleases).
    /// </summary>
    /// <typeparam name="T">Type of payload that will be returned by this element</typeparam>
    public interface IProvider<T> : ITaskElement
        where T : IPayload
    {
        /// <summary>
        /// Provide enumeration of elements
        /// </summary>
        /// <returns>Enumeration of objects of type <typeparamref name="T"/></returns>
        IEnumerable<T> Provide();
    }
}
