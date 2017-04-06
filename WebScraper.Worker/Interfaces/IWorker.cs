//-----------------------------------------------------------------------
// <copyright file="IWorker.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker
{
    /// <summary>
    /// Interface for worker for execute tasks
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Execute work
        /// </summary>
        void DoWork();
    }
}
