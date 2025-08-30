using System;
using Mahar.Common.Utilities;
using Xunit;

namespace Mahar.Common.Tests
{
    public class GuardTests
    {
        [Fact]
        public void NotNull_WhenNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNull<string>(null!, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_WhenEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty("", "param"));
        }

        [Fact]
        public void Valid_WhenInvalid_Throws()
        {
            Assert.Throws<ArgumentException>(() => Guard.Valid(5, x => x > 10, "param"));
        }
    }
}
