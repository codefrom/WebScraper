//-----------------------------------------------------------------------
// <copyright file="IConsumer.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Interfaces.TaskElements
{
    using Common;

    /// <summary>
    /// Interface for consumer. It's final node, it should consume payload and do some final processing with it (i.e. save to file or database etc).
    /// </summary>
    public interface IConsumer : ITaskElement
    {
        /// <summary>
        /// Consumes an payload of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="payload">Payload to be consumed</param>
        void Consume(IPayload payload);
    }
}
