// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable SA1121 // We use our own aliases since they differ per platform
#if TARGET_WINDOWS
using NativeType = System.UInt16;
#else
using NativeType = System.Int32;
#endif

namespace System.Runtime.InteropServices;

/// <summary>
/// Represents the <c>wchar_t</c> type in C and C++.
/// </summary>
/// <remarks>
/// <see cref="CWChar"/> is an exchange type for the managed/unmanaged boundary that accurately represents
/// unmanaged APIs that use the <c>wchar_t</c> type.
/// This type has 16 bits of unsigned storage on Windows, where <c>wchar_t</c> is a UTF-16 code unit.
/// It has 32 bits of signed storage on other platforms, where <c>wchar_t</c> is a UTF-32 code point.
/// </remarks>
[CLSCompliant(false)]
[Intrinsic]
public readonly struct CWChar : IEquatable<CWChar>
{
    private readonly NativeType _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CWChar"/> struct from a UTF-16 code unit.
    /// </summary>
    /// <param name="value">The UTF-16 code unit.</param>
    public CWChar(char value)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CWChar"/> struct from an integer.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying storage type.</exception>
    public CWChar(int value)
    {
        _value = checked((NativeType)value);
    }

    /// <summary>
    /// Gets the underlying integer value of this instance.
    /// </summary>
    /// <value>The underlying integer value of this instance.</value>
    public int Value => _value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="o">An object to compare with this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="o"/> is an instance of <see cref="CWChar"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals([NotNullWhen(true)] object? o) => o is CWChar other && Equals(other);

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="CWChar"/> value.
    /// </summary>
    /// <param name="other">A <see cref="CWChar"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(CWChar other) => _value == other._value;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => _value.GetHashCode();

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => _value.ToString();
}
