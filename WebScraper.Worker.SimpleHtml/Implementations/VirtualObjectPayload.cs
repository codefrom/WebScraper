//-----------------------------------------------------------------------
// <copyright file="VirtualObjectPayload.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using System.Collections.Generic;
    using System.Text;
    using Common;

    /// <summary>
    /// Class that implements simple name-value for mapping purposes
    /// </summary>
    public class VirtualObjectPayload : IPayload
    {
        /// <summary>
        /// Separator of class names and fields in string
        /// </summary>
        private const char ObjectsSeparator = '.';

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
            if (name.Contains(ObjectsSeparator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeparator }, 2);
                if (!this.references.ContainsKey(split[0]))
                {
                    this.references[split[0]] = new VirtualObjectPayload();
                }

                this.references[split[0]].SetProperty(split[1], value);
            }
            else
            {
                this.values[name] = value;
            }
        }

        /// <summary>
        /// Getting a property value
        /// </summary>
        /// <param name="name">Property name (through descendants maybe)</param>
        /// <returns>Returns property value, if not exists then empty</returns>
        public string GetProperty(string name)
        {
            if (name.Contains(ObjectsSeparator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeparator }, 2);
                if (!this.references.ContainsKey(split[0]))
                {
                    this.references[split[0]] = new VirtualObjectPayload();
                }

                return this.references[split[0]].GetProperty(split[1]);
            }
            else
            {
                if (!this.values.ContainsKey(name))
                {
                    return string.Empty;
                }

                return this.values[name];
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
            foreach (var value in this.values)
            {
                sb.AppendLine($"{value.Key}={value.Value}");
            }

            foreach (var reference in this.references)
            {
                sb.AppendLine($"{reference.Key}={reference.Value.ToString()}");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
