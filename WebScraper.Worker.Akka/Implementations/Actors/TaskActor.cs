//-----------------------------------------------------------------------
// <copyright file="TaskActor.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace WebScraper.Worker.Akka.Implementations.Actors
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Akka.Actor;
    using CodeFrom.WebScraper.Common;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using NLog;

    /// <summary>
    /// Actor for processing <seealso cref="ITaskElement"/>
    /// </summary>
    public class TaskActor : ReceiveActor
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Name of next actor to receive answer
        /// </summary>
        private string nextActorName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskActor" /> class with given <seealso cref="ITaskElement"/>
        /// </summary>
        /// <param name="element">Task element to be processed</param>
        /// <param name="nextActorName">Name of next actor to receive answer</param>
        public TaskActor(ITaskElement element, string nextActorName = null)
        {
            this.nextActorName = nextActorName;
            dynamic dynElement = element;
            this.Receive<IEnumerable<IPayload>>(payload => 
            {
                this.Execute(dynElement, payload);
            });
        }

        /// <summary>
        /// Initializes <seealso cref="Props"/> for actor creation
        /// </summary>
        /// <param name="element">Task element to be processed</param>
        /// <param name="nextActorName">Name of next actor to receive answer</param>
        /// <returns>Initialized props for actor</returns>
        public static Props Props(ITaskElement element, string nextActorName = null)
        {
            return global::Akka.Actor.Props.Create(() => new TaskActor(element, nextActorName));
        }

        /// <summary>
        /// After stop send PoisonPill further
        /// </summary>
        protected override void PostStop()
        {
            logger.Debug($"Stopping {this.Self.Path}, forwarding PoisonPill");
            base.PostStop();
            this.SendMessageFurther(PoisonPill.Instance);
        }

        /// <summary>
        /// Wrapping single payload into enumeration
        /// </summary>
        /// <param name="payload">Payload to be wrapped</param>
        /// <returns>Returns enumeration for this payload</returns>
        private IEnumerable<IPayload> PayloadToEnumerable(IPayload payload)
        {
            yield return payload;
            yield break;
        }

        /// <summary>
        /// Sends message to next actor
        /// </summary>
        /// <param name="result">Result to be send</param>
        private void SendMessageFurther(PoisonPill result)
        {
            if (!string.IsNullOrEmpty(this.nextActorName))
            {
                // get sibling by name
                var nextActor = Context.ActorSelection($"../{this.nextActorName}");
                nextActor.Tell(result);
            }
            else
            {
                // if no siblings left, terminate actor system
                Context.System.Terminate();
            }
        }

        /// <summary>
        /// Sends message to next actor
        /// </summary>
        /// <param name="result">Result to be send</param>
        private void SendMessageFurther(IPayload result)
        {
            this.SendMessageFurther(this.PayloadToEnumerable(result));
        }

        /// <summary>
        /// Sends message to next actor
        /// </summary>
        /// <param name="result">Result to be send</param>
        private void SendMessageFurther(IEnumerable<IPayload> result)
        {
            if (!string.IsNullOrEmpty(this.nextActorName))
            {
                // get sibling by name
                var nextActor = Context.ActorSelection($"../{this.nextActorName}");
                nextActor.Tell(result);
            }
        }

        /// <summary>
        /// Execute for ISplitter
        /// </summary>
        /// <param name="element">ISplitter task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(ISplitter<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for ISplitter element");
            foreach (var payload in payloads)
            {
                foreach (var result in element.Split(payload).AsParallel())
                {
                    this.SendMessageFurther(result);
                }
            }

            logger.Debug($"Executing for ISplitter element finished");
        }

        /// <summary>
        /// Execute for IAggregator
        /// </summary>
        /// <param name="element">IAggregator task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(IAggregator<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IAggregator element");
            var result = element.Aggregate(payloads);
            this.SendMessageFurther(result);
            logger.Debug($"Executing for IAggregator element finished");
        }

        /// <summary>
        /// Execute for IConsumer
        /// </summary>
        /// <param name="element">IConsumer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(IConsumer element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IConsumer element");
            foreach (var payload in payloads)
            {
                element.Consume(payload);
            }

            logger.Debug($"Executing for IConsumer element finished");
        }

        /// <summary>
        /// Execute for IProvider
        /// </summary>
        /// <param name="element">IProvider task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(IProvider<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IProvider element");
            foreach (var result in element.Provide().AsParallel())
            {
                this.SendMessageFurther(result);
            }

            this.Self.Tell(PoisonPill.Instance);
            logger.Debug($"Executing for IProvider element finished");
        }

        /// <summary>
        /// Execute for ITransformer
        /// </summary>
        /// <param name="element">ITransformer task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(ITransformer<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for ITransformer element");
            foreach (var payload in payloads)
            {
                var result = element.Transform(payload);
                this.SendMessageFurther(result);
            }

            logger.Debug($"Executing for ITransformer element finished");
        }

        /// <summary>
        /// Execute for IExtractor
        /// </summary>
        /// <param name="element">IExtractor task</param>
        /// <param name="payloads">Enumeration of payloads (may be one, empty or many)</param>
        private void Execute(IExtractor<IPayload> element, IEnumerable<IPayload> payloads)
        {
            logger.Debug($"Executing for IExtractor element");
            foreach (var payload in payloads)
            {
                var result = element.ExtractFrom(payload);
                this.SendMessageFurther(result);
            }

            logger.Debug($"Executing for IExtractor element finished");
        }
    }
}
