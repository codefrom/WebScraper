//-----------------------------------------------------------------------
// <copyright file="VirtualObjectPayload.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using Common;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class that implements simple name-value for mapping purposes
    /// </summary>
    public class VirtualObjectPayload : IPayload
    {
        /// <summary>
        /// Seporator of class names and fields in string
        /// </summary>
        private const char ObjectsSeporator = '.';

        /// <summary>
        /// Dictionary of simple values
        /// </summary>
        private Dictionary<string, string> values = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary for virtual object values
        /// </summary>
        private Dictionary<string, VirtualObjectPayload> references = new Dictionary<string, VirtualObjectPayload>();

        /// <summary>
        /// Setting a value for property
        /// </summary>
        /// <param name="name">Property name (through descendants maybe)</param>
        /// <param name="value">Value to set</param>
        public void SetProperty(string name, string value)
        {
            if (name.Contains(ObjectsSeporator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeporator }, 2);
                if (!references.ContainsKey(split[0]))
                {
                    references[split[0]] = new VirtualObjectPayload();
                }
                references[split[0]].SetProperty(split[1], value);
            }
            else
            {
                values[name] = value;
            }
        }

        /// <summary>
        /// Getting a property value
        /// </summary>
        /// <param name="name">Property name (through descendants maybe)</param>
        /// <returns>Returns property value, if not exists then empty</returns>
        public string GetProperty(string name)
        {
            if (name.Contains(ObjectsSeporator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeporator }, 2);
                if (!references.ContainsKey(split[0]))
                {
                    references[split[0]] = new VirtualObjectPayload();
                }
                return references[split[0]].GetProperty(split[1]);
            }
            else
            {
                if (!values.ContainsKey(name))
                {
                    return string.Empty;
                }
                return values[name];
            }
        }

        /// <summary>
        /// Serializes VirtualObject to some string format
        /// </summary>
        /// <returns>String representation of object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var value in values)
            {
                sb.AppendLine($"{value.Key}={value.Value}");
            }
            foreach (var reference in references)
            {
                sb.AppendLine($"{reference.Key}={reference.Value.ToString()}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
