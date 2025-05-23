﻿// <auto-generated>
// This file is referenced as external source. Do not modify.
// </auto-generated>

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(CONSTANT_EXPECTED_ATTRIBUTE_EXTERNAL || NET7_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Indicates that the specified method parameter expects a constant.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class ConstantExpectedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantExpectedAttribute"/> class.
        /// </summary>
        public ConstantExpectedAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the minimum bound of the expected constant, inclusive.
        /// </summary>
        public object? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum bound of the expected constant, inclusive.
        /// </summary>
        public object? Max { get; set; }
    }
}
#endif

#if !(EXCLUDE_FROM_CODE_COVERAGE_ATTRIBUTE || NETCOREAPP3_0_OR_GREATER)
#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    // Excludes the attributed code from code coverage information.
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
