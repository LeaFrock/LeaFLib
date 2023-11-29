using System;
using Xunit;

namespace LeaFLib.Common.Core.UnitTest
{
    public class StringHelperUnitTest
    {
        [Theory]
        [InlineData(0, typeof(ArgumentOutOfRangeException))]
        [InlineData(7, typeof(ArgumentException))]
        [InlineData(4, default)]
        [InlineData(1024, default)]
        public void RandomBase64String(int length, Type? exceptionType)
        {
            if (exceptionType is null)
            {
                string result = StringHelper.RandomBase64String(length);
                Assert.Equal(length, result.Length);
            }
            else
            {
                Assert.Throws(exceptionType, () => StringHelper.RandomBase64String(length));
            }
        }
    }
}