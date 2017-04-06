//-----------------------------------------------------------------------
// <copyright file="ITask.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.Interfaces
{
    /// <summary>
    /// Interface for task representation
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Execute task (in common does not return any value)
        /// </summary>
        void DoTask();
    }
}
