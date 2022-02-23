// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !(NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif

namespace System
{
    /// <summary>
    /// Represents a type that can be used to index a collection either from the start or the end.
    /// </summary>
    public readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new <see cref="Index"/> with a specified index position and a value that
        /// indicates if the index is from the start or the end of a collection.
        /// </summary>
        /// <param name="value">The index value. It has to be greater or equal than zero.</param>
        /// <param name="fromEnd">A boolean indicating if the index is from the start (<see langword="false"/>)
        /// or from the end (<see langword="true"/>) of a collection.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Non-negative number required.");
            }

            if (fromEnd)
                this._value = ~value;
            else
                this._value = value;
        }

        // The following private constructors mainly created for perf reason to avoid the checks
        private Index(int value)
        {
            this._value = value;
        }

        /// <summary>
        /// Gets an <see cref="Index"/> that points to the first element of a collection.
        /// </summary>
        /// <returns>An instance that points to the first element of a collection.</returns>
        public static Index Start => new Index(0);

        /// <summary>
        /// Gets an <see cref="Index"/> that points beyond the last element.
        /// </summary>
        /// <returns>an <see cref="Index"/> that points beyond the last element.</returns>
        public static Index End => new Index(~0);

        /// <summary>
        /// Create an <see cref="Index"/> from the specified index at the start of a collection.
        /// </summary>
        /// <param name="value">The index position from the start of a collection.</param>
        /// <returns>The Index value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Non-negative number required.");
            }

            return new Index(value);
        }

        /// <summary>
        /// Creates an <see cref="Index"/> from the end of a collection at a specified index position.
        /// </summary>
        /// <param name="value">The index value from the end of a collection.</param>
        /// <returns>The Index value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromEnd(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Non-negative number required.");
            }

            return new Index(~value);
        }

        /// <summary>
        /// Gets the index value.
        /// </summary>
        /// <returns>The index value.</returns>
        public int Value
        {
            get
            {
                if (this._value < 0)
                    return ~this._value;
                else
                    return this._value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the index is from the start or the end.
        /// </summary>
        /// <returns><see langword="true"/> if the Index is from the end;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsFromEnd => this._value < 0;

        /// <summary>
        /// Calculates the offset from the start of the collection using the given collection length.
        /// </summary>
        /// <param name="length">The length of the collection that the Index will be used with.
        /// Must be a positive value.</param>
        /// <returns>The offset.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = this._value;
            if (this.IsFromEnd)
            {
                // offset = length - (~value)
                // offset = length + (~(~value) + 1)
                // offset = length + value + 1

                offset += length + 1;
            }
            return offset;
        }

        /// <summary>
        /// Indicates whether the current Index object is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is of type <see cref="Index"/>
        /// and is equal to the current instance; <see langword="false"/> otherwise.</returns>
        public override bool Equals([NotNullWhen(true)] object? value) =>
            value is Index && this._value == ((Index)value)._value;

        /// <summary>
        /// Returns a value that indicates whether the current object is equal to another <see cref="Index"/> object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current Index object is equal to <paramref name="other"/>;
        /// <see langword="false"/> otherwise.</returns>
        public bool Equals(Index other) => this._value == other._value;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => this._value;

        /// <summary>
        /// Converts integer number to an Index.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>An Index representing the integer.</returns>
        public static implicit operator Index(int value) => Index.FromStart(value);

        /// <summary>
        /// Returns the string representation of the current <see cref="Index"/> instance.
        /// </summary>
        /// <returns>The string representation of the <see cref="Index"/>.</returns>
        public override string ToString()
        {
            if (this.IsFromEnd)
                return this.ToStringFromEnd();

            return ((uint)this.Value).ToString();
        }

        private string ToStringFromEnd()
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            Span<char> span = stackalloc char[11]; // 1 for ^ and 10 for longest possible uint value
            bool formatted = ((uint)this.Value).TryFormat(span.Slice(1), out int charsWritten);
            Debug.Assert(formatted);
            span[0] = '^';
            return new string(span.Slice(0, charsWritten + 1));
#else
            return '^' + this.Value.ToString();
#endif
        }
    }

    /// <summary>
    /// Represents a range that has start and end indexes.
    /// </summary>
    public readonly struct Range : IEquatable<Range>
    {
        /// <summary>
        /// Gets the inclusive start index of the <see cref="Range"/>.
        /// </summary>
        /// <returns>The inclusive start index of the range.</returns>
        public Index Start { get; }

        /// <summary>
        /// Gets an <see cref="Index"/> that represents the exclusive end index of the range.
        /// </summary>
        /// <returns>The end index of the range.</returns>
        public Index End { get; }

        /// <summary>
        /// Instantiates a new <see cref="Range"/> instance with the specified starting and ending indexes.
        /// </summary>
        /// <param name="start">The inclusive start index of the range.</param>
        /// <param name="end">The exclusive end index of the range.</param>
        public Range(Index start, Index end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this Range object.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is of type <see cref="Range"/>
        /// and is equal to the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals([NotNullWhen(true)] object? value) =>
            value is Range range && range.Start.Equals(this.Start) && range.End.Equals(this.End);

        /// <summary>
        /// Returns a value that indicates whether the current instance
        /// is equal to another <see cref="Range"/> object.
        /// </summary>
        /// <param name="other">A Range object to compare with this Range object.</param>
        /// <returns><see langword="true"/> if the current instance is equal to <paramref name="other"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return HashCode.Combine(this.Start.GetHashCode(), this.End.GetHashCode());
#else
            int h1 = this.Start.GetHashCode(), h2 = this.End.GetHashCode();
            // RyuJIT optimizes this to use the ROL instruction
            // Related GitHub pull request: https://github.com/dotnet/coreclr/pull/1830
            uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)rol5 + h1) ^ h2;
#endif
        }

        /// <summary>
        /// Returns the string representation of the current <see cref="Range"/> object.
        /// </summary>
        /// <returns>The string representation of the range.</returns>
        public override string ToString()
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            Span<char> span = stackalloc char[2 + (2 * 11)]; // 2 for "..", then for each index 1 for '^' 
                                                             //   and 10 for longest possible uint
            int pos = 0;

            if (this.Start.IsFromEnd)
            {
                span[0] = '^';
                pos = 1;
            }
            bool formatted = ((uint)this.Start.Value).TryFormat(span.Slice(pos), out int charsWritten);
            Debug.Assert(formatted);
            pos += charsWritten;

            span[pos++] = '.';
            span[pos++] = '.';

            if (this.End.IsFromEnd)
            {
                span[pos++] = '^';
            }
            formatted = ((uint)this.End.Value).TryFormat(span.Slice(pos), out charsWritten);
            Debug.Assert(formatted);
            pos += charsWritten;

            return new string(span.Slice(0, pos));
#else
            return this.Start.ToString() + ".." + this.End.ToString();
#endif
        }

        /// <summary>
        /// Returns a new <see cref="Range"/> instance starting from
        /// a specified start index to the end of the collection.
        /// </summary>
        /// <param name="start">The position of the first element from which the Range will be created.</param>
        /// <returns>A range from <paramref name="start"/> to the end of the collection.</returns>
        public static Range StartAt(Index start) => new Range(start, Index.End);

        /// <summary>
        /// Creates a <see cref="Range"/> object starting from the first element
        /// in the collection to a specified end index.
        /// </summary>
        /// <param name="end">The position of the last element up to
        /// which the <see cref="Range"/> object will be created.</param>
        /// <returns>A range that starts from the first element to <paramref name="end"/>.</returns>
        public static Range EndAt(Index end) => new Range(Index.Start, end);

        /// <summary>
        /// Gets a <see cref="Range"/> object that starts from the first element to the end.
        /// </summary>
        /// <returns>A range from the start to the end.</returns>
        public static Range All => new Range(Index.Start, Index.End);

        /// <summary>
        /// Calculates the start offset and length of the range object using a collection length.
        /// </summary>
        /// <param name="length">A positive integer that represents the length of the collection
        /// that the range will be used with.</param>
        /// <returns>The start offset and length of the range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is outside the bounds of the current range.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start;
            Index startIndex = this.Start;
            if (startIndex.IsFromEnd)
                start = length - startIndex.Value;
            else
                start = startIndex.Value;

            int end;
            Index endIndex = this.End;
            if (endIndex.IsFromEnd)
                end = length - endIndex.Value;
            else
                end = endIndex.Value;

            if ((uint)end > (uint)length || (uint)start > (uint)end)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return (start, end - start);
        }
    }
}
#endif
