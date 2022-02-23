// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NETCOREAPP3_0_OR_GREATER
using System.Diagnostics;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Allows capturing of the expressions passed to a method.
    /// </summary>
    [CompilerGenerated, DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the targeted parameter.</param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the target parameter name of the CallerArgumentExpression.
        /// </summary>
        /// <returns>The name of the targeted parameter of the CallerArgumentExpression.</returns>
        public string ParameterName { get; }
    }
}
#endif
