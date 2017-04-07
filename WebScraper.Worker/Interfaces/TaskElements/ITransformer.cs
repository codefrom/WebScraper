//-----------------------------------------------------------------------
// <copyright file="ITransformer.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Interfaces.TaskElements
{
    using Common;

    /// <summary>
    /// Interface for transformer task element
    /// </summary>
    /// <typeparam name="T">Type of payload that will be returned by this element</typeparam>
    public interface ITransformer<out T> : ITaskElement
        where T : IPayload
    {
        /// <summary>
        /// Transforms payload from another payload
        /// </summary>
        /// <param name="payload">Payload of type <typeparamref name="K"/> that will be processed</param>
        /// <returns>One object of type <typeparamref name="T"/></returns>
        T Transform(IPayload payload);
    }
}
