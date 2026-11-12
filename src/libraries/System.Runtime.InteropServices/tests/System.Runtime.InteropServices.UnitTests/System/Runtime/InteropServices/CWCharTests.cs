// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit;

namespace System.Runtime.InteropServices.Tests
{
    public class CWCharTests
    {
        [Fact]
        public void Ctor_Empty()
        {
            CWChar value = new CWChar();
            Assert.Equal(0, value.Value);
        }

        [Theory]
        [InlineData('\0')]
        [InlineData('A')]
        [InlineData('\u00E9')]
        [InlineData('\uFFFD')]
        [InlineData('\uFFFF')]
        public void Ctor_Char(char c)
        {
            CWChar value = new CWChar(c);
            Assert.Equal(c, value.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0x41)]
        [InlineData(0x8000)]
        [InlineData(0xD800)]
        [InlineData(0xFFFF)]
        public void Ctor_Int(int i)
        {
            CWChar value = new CWChar(i);
            Assert.Equal(i, value.Value);
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsWindows))]
        [InlineData(-1)]
        [InlineData(0x10000)]
        [InlineData(0x10FFFF)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Ctor_Int_OutOfRange(int i)
        {
            Assert.Throws<OverflowException>(() => new CWChar(i));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsNotWindows))]
        [InlineData(-1)]
        [InlineData(0x10000)]
        [InlineData(0x10FFFF)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Ctor_Int_LargeValue(int i)
        {
            CWChar value = new CWChar(i);
            Assert.Equal(i, value.Value);
        }

        public static IEnumerable<object[]> EqualsData()
        {
            yield return new object[] { new CWChar('a'), new CWChar('a'), true };
            yield return new object[] { new CWChar('a'), new CWChar(0x61), true };
            yield return new object[] { new CWChar('a'), new CWChar('A'), false };
            yield return new object[] { new CWChar('a'), new CWChar(), false };
            yield return new object[] { new CWChar(0), new CWChar(), true };
            yield return new object[] { new CWChar(0xFFFF), new CWChar('\uFFFF'), true };
            yield return new object[] { new CWChar('a'), null, false };
            yield return new object[] { new CWChar('a'), "a", false };
            yield return new object[] { new CWChar('a'), 'a', false };
            yield return new object[] { new CWChar('a'), 0x61, false };
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void EqualsTest(CWChar cwchar, object obj, bool expected)
        {
            if (obj is CWChar cwchar2)
            {
                Assert.Equal(expected, cwchar.Equals(cwchar2));
                Assert.Equal(expected, cwchar.GetHashCode().Equals(cwchar2.GetHashCode()));
            }
            Assert.Equal(expected, cwchar.Equals(obj));
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(0x41, "65")]
        [InlineData(0xFFFF, "65535")]
        public static void ToStringTest(int value, string expected)
        {
            CWChar cwchar = new CWChar(value);

            Assert.Equal(expected, cwchar.ToString());
        }

        [Fact]
        public unsafe void Size()
        {
            int size = PlatformDetection.IsWindows ? 2 : 4;
#pragma warning disable xUnit2000 // The value under test here is the sizeof expression
            Assert.Equal(size, sizeof(CWChar));
#pragma warning restore xUnit2000
            Assert.Equal(size, Marshal.SizeOf<CWChar>());
        }
    }
}
