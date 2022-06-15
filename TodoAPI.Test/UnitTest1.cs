using Shouldly;

namespace TodoAPI.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1, 1);     
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(1, 2);
        }

        [Theory(DisplayName = "Add Numbers")]
        [InlineData(4, 5, 9)]
        [InlineData(2, 3, 5)]
        public void TestAddNumbers(int x, int y, int expectedResult)
        {
            var result = x + y;
            Assert.Equal(expectedResult, result);
        }
    }
}