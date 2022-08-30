using Moq;

namespace SparkyXUnitTest;

public class ProductXUnitTests
{
    [Fact]
    public void GetProductPrice_PlatinumCustomer_ReturnPriceWith20Discount()
    {
        Product product = new Product() { Price = 50 };
        var result = product.GetPrice(new Customer() { IsPlatinum = true });

        Assert.Equal(40, result);
    }

    [Fact]
    // MOQ Abuse
    public void GetProductPrice_MOQAbuse_PlatinumCustomer_ReturnPriceWith20Discount()
    {
        var customer = new Mock<ICustomer>();
        customer.Setup(u => u.IsPlatinum).Returns(true);

        Product product = new Product() { Price = 50 };

        var result = product.GetPrice(customer.Object);

        Assert.Equal(40, result);
    }
}
