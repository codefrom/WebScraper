//-----------------------------------------------------------------------
// <copyright file="Provider.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System;
    using System.Collections.Generic;
    using CsQuery;
    using Interfaces.TaskElements;
    using NLog;

    /// <summary>
    /// Simple http provider
    /// </summary>
    public class Provider : IProvider<HtmlPayload>
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets address to be loaded
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets depth of crawling
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets next url selector
        /// </summary>
        public ValueSelector NextUrlSelector { get; set; }

        /// <summary>
        /// Provide payload from link
        /// </summary>
        /// <returns>Returns payload of loaded page</returns>
        public IEnumerable<HtmlPayload> Provide()
        {
            logger.Debug($"Start providing");
            var first = new HtmlPayload()
            {
                Address = new Uri(this.Address),
                Content = CQ.CreateFromUrl(this.Address)
            };
            logger.Debug($"Providing {first.Address} html");
            yield return first;

            var stack = new Stack<HtmlPayload>();
            var newStack = new Stack<HtmlPayload>();
            var currentDepth = 0;
            stack.Push(first);
            while (currentDepth < this.MaxDepth)
            {
                while (stack.Count > 0)
                {
                    var payload = stack.Pop();
                    foreach (var nextAddress in this.NextUrlSelector.SelectMany(payload.Content))
                    {
                        if (string.IsNullOrEmpty(nextAddress))
                        {
                            continue;
                        }

                        var nextUri = new Uri(payload.Address, nextAddress);
                        var next = new HtmlPayload()
                        {
                            Address = nextUri,
                            Content = CQ.CreateFromUrl(nextUri.ToString())
                        };
                        logger.Debug($"Providing {next.Address} html");
                        yield return next;
                        newStack.Push(next);
                    }
                }

                currentDepth++;
                stack = newStack;
            }

            logger.Debug($"Finishing provide");
            yield break;
        }
    }
}
