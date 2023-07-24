using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFileSystem.Collections;

namespace SharpFileSystem.Tests.Collections
{
    [TestClass]
    public class EnumerableCollectionTests
    {
        [TestMethod]
        public void When_CopyToArray_Expect_OutputEqualToInput()
        {
            var input = new[] {"a", "b", "c"};
            var enumerableCollection = new EnumerableCollection<string>(input, input.Length);
            var output = new string[3];
            enumerableCollection.CopyTo(output, 0);
            output.Should().BeEquivalentTo(input);
        }

        [TestMethod]
        public void When_CopyToTooSmallArray_Expect_ArgumentOutOfRangeException()
        {
            var input = new[] {"a", "b", "c"};
            var enumerableCollection = new EnumerableCollection<string>(input, input.Length);
            var output = new string[2];
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                enumerableCollection.CopyTo(output, 0);
            });
        }

        [TestMethod]
        public void When_CopyToInvalidIndex_Expect_ArgumentOutOfRangeException()
        {
            var input = new[] {"a", "b", "c"};
            var enumerableCollection = new EnumerableCollection<string>(input, input.Length);
            var output = new string[3];
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                enumerableCollection.CopyTo(output, 1);
            });
        }
    }
}
