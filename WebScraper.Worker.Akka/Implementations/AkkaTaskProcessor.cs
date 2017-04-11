//-----------------------------------------------------------------------
// <copyright file="AkkaTaskProcessor.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace WebScraper.Worker.Akka.Implementations
{
    using System;
    using System.Collections.Generic;
    using Actors;
    using global::Akka.Actor;
    using CodeFrom.WebScraper.Common;
    using CodeFrom.WebScraper.Worker.Interfaces;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using NLog;

    /// <summary>
    /// Task implementation, using actors
    /// </summary>
    public class AkkaTaskProcessor : ITask, IDisposable
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Entry point of actors system
        /// </summary>
        private IActorRef actorEntryPoint = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AkkaTaskProcessor" /> class
        /// </summary>
        /// <param name="name">Name of current instance, by default is <code>"AkkaTaskProcessor"</code></param>
        public AkkaTaskProcessor(string name = "AkkaTaskProcessor")
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets name of current instance
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets TaskElements that will be executed in this task
        /// </summary>
        public List<ITaskElement> TaskElements { get; set; }

        /// <summary>
        /// Gets or sets actor system for this task
        /// </summary>
        protected ActorSystem Actors { get; set; }
        
        /// <summary>
        /// Execute current task
        /// </summary>
        public void DoTask()
        {
            logger.Debug($"Started AkkaTaskProcessor");
            if (this.Actors == null)
            {
                logger.Debug($"Initializing actors system");
                this.Actors = ActorSystem.Create(this.Name);
                this.TaskElements.Reverse();
                string prevActorName = null;
                int actorIndex = 0;
                
                foreach (var element in this.TaskElements)
                {
                    var currentActorName = $"{element.GetType().Name}_{actorIndex}";
                    this.actorEntryPoint = this.Actors.ActorOf(TaskActor.Props(element, prevActorName), currentActorName);
                    prevActorName = currentActorName;
                    actorIndex++;
                }

                logger.Debug($"Initialized actors system");
            }

            // start processing with empty message
            this.actorEntryPoint?.Tell(new List<IPayload>());
            this.Actors.WhenTerminated.Wait();
            logger.Debug($"Finished AkkaTaskProcessor");
        }

        /// <summary>
        /// Dispose current instance of class
        /// </summary>
        public void Dispose()
        {
            if (this.Actors != null)
            {
                this.Actors.Terminate().Wait();
            }
        }
    }
}
