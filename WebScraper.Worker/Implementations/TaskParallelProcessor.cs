//-----------------------------------------------------------------------
// <copyright file="TaskParallelProcessor.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Interfaces;
    using Interfaces.TaskElements;

    /// <summary>
    /// Simple task parallel executor
    /// </summary>
    public class TaskParallelProcessor : ITask
    {
        /// <summary>
        /// Gets or sets TaskElements that will be executed in this task
        /// </summary>
        public IEnumerable<ITaskElement> TaskElements { get; set; }

        /// <summary>
        /// Gets or sets degree of parallelism which wil be when executing over a enumerable
        /// </summary>
        public int DegreeOfParallelism { get; set; }

        /// <summary>
        /// Execute tasks
        /// </summary>
        public void DoTask()
        {
            IEnumerable<IPayload> payloads = null;
            foreach (var taskElement in this.TaskElements)
            {
                dynamic dynTaskElement = taskElement;
                payloads = this.Materialize(this.Execute(dynTaskElement, payloads));
            }
        }

        /// <summary>
        /// Materializes enumeration to next execution step
        /// </summary>
        /// <param name="payloads">Enumeration of payloads to be materialized</param>
        /// <returns>Materialized enumeration (list)</returns>
        private IEnumerable<IPayload> Materialize(IEnumerable<IPayload> payloads)
        {
            return payloads
                .AsParallel()
                .WithDegreeOfParallelism(this.DegreeOfParallelism > 0 ? this.DegreeOfParallelism : 1)
                .ToList();
        }

        /// <summary>
        /// Execute for ISplitter
        /// </summary>
        /// <param name="element">ISplitter task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(ISplitter<IPayload> element, IEnumerable<IPayload> payloads)
        {
            foreach (var payload in payloads)
            {
                foreach (var ret in element.Split(payload))
                {
                    yield return ret;
                }
            }

            yield break;
        }

        /// <summary>
        /// Execute for IAggregator
        /// </summary>
        /// <param name="element">IAggregator task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IAggregator<IPayload> element, IEnumerable<IPayload> payloads)
        {
            yield return element.Aggregate(payloads);
        }

        /// <summary>
        /// Execute for IConsumer
        /// </summary>
        /// <param name="element">IConsumer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IConsumer element, IEnumerable<IPayload> payloads)
        {
            foreach (var payload in payloads)
            {
                element.Consume(payload);
            }

            yield break;
        }

        /// <summary>
        /// Execute for IProvider
        /// </summary>
        /// <param name="element">IProvider task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IProvider<IPayload> element, IEnumerable<IPayload> payloads)
        {
            foreach (var ret in element.Provide())
            {
                yield return ret;
            }
        }

        /// <summary>
        /// Execute for ITransformer
        /// </summary>
        /// <param name="element">ITransformer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(ITransformer<IPayload> element, IEnumerable<IPayload> payloads)
        {
            foreach (var payload in payloads)
            {
                yield return element.Transform(payload);
            }

            yield break;
        }

        /// <summary>
        /// Execute for IExtractor
        /// </summary>
        /// <param name="element">IExtractor task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IExtractor<IPayload> element, IEnumerable<IPayload> payloads)
        {
            foreach (var payload in payloads)
            {
                foreach (var ret in element.ExtractFrom(payload))
                {
                    yield return ret;
                }
            }

            yield break;
        }
    }
}
