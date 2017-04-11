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
    using NLog;

    /// <summary>
    /// Simple task parallel executor
    /// </summary>
    public class TaskParallelProcessor : ITask
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Debug($"Starting task execution");
            IEnumerable<IPayload> payloads = null;
            foreach (var taskElement in this.TaskElements)
            {
                logger.Debug($"Processing {taskElement.GetType()}");
                dynamic dynTaskElement = taskElement;
                payloads = this.Execute(dynTaskElement, payloads);
                logger.Debug($"Processed {taskElement.GetType()}");
            }

            logger.Debug($"Finished task execution");
        }

        /// <summary>
        /// Prepares enumeration for parallel execution
        /// </summary>
        /// <param name="payloads">Enumeration of payloads to be parallel</param>
        /// <returns>Prepared enumeration</returns>
        private ParallelQuery<IPayload> AsParallel(IEnumerable<IPayload> payloads)
        {
            var parallel = payloads.AsParallel();
            if (this.DegreeOfParallelism > 0)
            {
                parallel = parallel.WithDegreeOfParallelism(this.DegreeOfParallelism);
            }

            return parallel;
        }

        /// <summary>
        /// Execute for ISplitter
        /// </summary>
        /// <param name="element">ISplitter task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(ISplitter<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for ISplitter element");
            return this.AsParallel(payloads).SelectMany(payload => element.Split(payload));
        }

        /// <summary>
        /// Execute for IAggregator
        /// </summary>
        /// <param name="element">IAggregator task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IAggregator<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IAggregator element");
            yield return element.Aggregate(payloads);
            logger.Debug($"Executing for IAggregator element finished");
            yield break;
        }

        /// <summary>
        /// Execute for IConsumer
        /// </summary>
        /// <param name="element">IConsumer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IConsumer element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IConsumer element");
            this.AsParallel(payloads).ForAll(payload => element.Consume(payload));
            logger.Debug($"Executing for IConsumer element finished");
            return payloads;
        }

        /// <summary>
        /// Execute for IProvider
        /// </summary>
        /// <param name="element">IProvider task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IProvider<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IProvider element");
            return element.Provide();
        }

        /// <summary>
        /// Execute for ITransformer
        /// </summary>
        /// <param name="element">ITransformer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(ITransformer<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for ITransformer element");
            return this.AsParallel(payloads).Select(payload => element.Transform(payload));
        }

        /// <summary>
        /// Execute for IExtractor
        /// </summary>
        /// <param name="element">IExtractor task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        /// <returns>Resulting enumeration of payloads (may be one, empty or many)</returns>
        private IEnumerable<IPayload> Execute(IExtractor<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IExtractor element");
            foreach (var result in this.AsParallel(payloads).SelectMany(payload => element.ExtractFrom(payload)))
            {
                yield return result;
            }
        }
    }
}
