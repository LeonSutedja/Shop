using Shouldly;
using Xunit;

namespace Shop.Order.Test
{
    public class OrderServiceTest
    {
        public OrderServiceTest()
        {
            //    var unityContainer = new UnityContainer();

            //    Shop.App_Start.UnityConfig.RegisterTypes(unityContainer as (Microsoft.Practices.Unity.UnityContainer));
        }

        [Fact]
        public void PassingTest()
        {
            var a = 4;
            a.ShouldBe(4);
        }
    }
}