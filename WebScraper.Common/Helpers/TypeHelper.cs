//-----------------------------------------------------------------------
// <copyright file="TypeHelper.cs" company="CodeFrom">
//     Copyright (c) CodeFrom. All rights reserved.
//     https://github.com/codefrom/
// </copyright>
// <author>Rateev Ilya</author>
//-----------------------------------------------------------------------
namespace CodeFrom.WebScraper.Common.Helpers
{
    using System;
    using System.Linq;

    /// <summary>
    /// Helper for Type class
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Checks if Type is implementation of interface
        /// </summary>
        /// <param name="baseType">Input Type</param>
        /// <param name="interfaceType">Interface that implementation we are searching</param>
        /// <returns>Returns true if type implements given interface</returns>
        public static bool IsImplementationOf(this Type baseType, Type interfaceType)
        {
            if (interfaceType.IsGenericTypeDefinition)
            {
                return baseType.GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == interfaceType);
            }

            return baseType.GetInterfaces().Any(interfaceType.Equals);
        }
    }
}
