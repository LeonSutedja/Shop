using Shouldly;
using Xunit;

namespace Shop.Order.Test
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            var a = 4;
            a.ShouldBe(4);
        }
    }
}