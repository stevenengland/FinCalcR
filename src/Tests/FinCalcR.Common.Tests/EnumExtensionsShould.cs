using System;
using FluentAssertions;
using StEn.FinCalcR.Common.Extensions;
using Xunit;

namespace FinCalcR.Common.Tests
{
    [Flags]
    public enum Test
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 4,
    }

    public class EnumExtensionsShould
    {
        [Fact]
        public void ReturnTrue_WhenCheckingIfOnlyThisFlagIsNotSet_AndOnlyFlagFlagIsNotSet()
        {
            const Test flags = Test.First | Test.Second;

            flags.IsOnlyFlagNotSet(Test.Third).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenCheckingIfOnlyThisFlagIsNotSet_AndMoreThanThisFlagIsNotSet()
        {
            const Test flags = Test.First;

            flags.IsOnlyFlagNotSet(Test.Third).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenCheckingIfOnlyThisFlagIsSet_AndOnlyThisFlagIsSet()
        {
            const Test flags = Test.Third;

            flags.IsOnlyFlagSet(Test.Third).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenCheckingIfOnlyThisFlagIsSet_AndThisFlagIsNotSet()
        {
            const Test flags = Test.First;

            flags.IsOnlyFlagSet(Test.Third).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenCheckingIfEveryFlagIsSet_AndAllFlagsAreSet()
        {
            const Test flags = Test.First | Test.Second | Test.Third;

            flags.IsEveryFlagSet().Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenCheckingIfEveryFlagIsSet_AndNotAllFlagsAreSet()
        {
            const Test flags = Test.First | Test.Second;

            flags.IsEveryFlagSet().Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenCheckingIfNoFlagIsSet_AndNoFlagIsSet()
        {
            const Test flags = Test.None;

            flags.IsNoFlagSet().Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenCheckingIfNoFlagIsSet_AndAFlagIsSet()
        {
            const Test flags = Test.Second;

            flags.IsNoFlagSet().Should().BeFalse();
        }

        [Fact]
        public void SetAllFlags()
        {
            // Arrange
            var flags = Test.None;

            // Act
            flags = flags.SetAllFlags(true);

            // Assert
            flags.HasFlag(Test.First).Should().BeTrue();
            flags.HasFlag(Test.Second).Should().BeTrue();
            flags.HasFlag(Test.Third).Should().BeTrue();
        }

        [Fact]
        public void UnSetAllFlags()
        {
            // Arrange
            var flags = Test.First | Test.Second | Test.Third;

            // Act
            flags = flags.SetAllFlags(false);

            // Assert
            flags.HasFlag(Test.First).Should().BeFalse();
            flags.HasFlag(Test.Second).Should().BeFalse();
            flags.HasFlag(Test.Third).Should().BeFalse();
        }
    }
}
