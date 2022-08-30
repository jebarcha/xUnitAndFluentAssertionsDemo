using FluentAssertions;
using FluentAssertions.Execution;
using System;

namespace SparkyXUnitTest;
public class CustomerXUnitTests
{
    private Customer customer;
    public CustomerXUnitTests()
    {
        customer = new();
    }


    [Fact]
    public void CombineNames_InputFirstAndLastName_ReturnFullName()
    {
        //Arrange
        //var customer = new Customer();

        //Act
        customer.GreetAndCombineNames("Jose", "Barajas");

        //Assert
        //Assert.Equal("Hello, Jose Barajas", customer.GreetMessage);
        //Assert.StartsWith("Hello", customer.GreetMessage);
        //Assert.EndsWith("Barajas", customer.GreetMessage);
        //Assert.Contains("jose", customer.GreetMessage, StringComparison.InvariantCultureIgnoreCase);
        //Assert.Matches("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", customer.GreetMessage);

        using (new AssertionScope())
        {
            customer.GreetMessage.Should().Be("Hello, Jose Barajas", "Mismatch in Score1!");
            customer.GreetMessage.Should().NotBe("Hello, Jesus Barajas", "Mismatch in Score2!");
            customer.GreetMessage.Should().StartWith("Hello");
            customer.GreetMessage.Should().EndWith("Barajas");
            customer.GreetMessage.Should().Contain("Jose");
            customer.GreetMessage.Should().MatchRegex("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+");
        }

    }

    [Fact]
    public void GreetMessage_NotGreeted_ReturnNull()
    {
        //Arrange
        //var customer = new Customer();

        //Act

        //Assert
        Assert.Null(customer.GreetMessage);
    }

    [Fact]
    public void DisccountCheck_DefaultCustomer_ReturnDiscountInRange()
    {
        int result = customer.Disccount;
        //Assert.Equal(15, result);
        Assert.InRange(result, 10, 25);
    }

    [Fact]
    public void GreetMessage_GreetedWithoutLastName_ReturnsNotNull()
    {
        customer.GreetAndCombineNames("jose", "");

        Assert.NotNull(customer.GreetMessage);
        Assert.False(string.IsNullOrEmpty(customer.GreetMessage));
    }

    [Fact]
    public void GreetMessage_GretedWithoutFirstName_ThrowsException()
    {
        var exceptionDetails = Assert.Throws<ArgumentException>(() => customer.GreetAndCombineNames("", "Smith"));
        Assert.Equal("Empty First Name", exceptionDetails.Message);

        //FluentAssertion
        customer.Invoking(y => y.GreetAndCombineNames("", "Smith"))
            .Should().Throw<ArgumentException>()
            .WithMessage("Empty First Name");

        Action act = () => customer.GreetAndCombineNames("", "Smith");
        act.Should().Throw<ArgumentException>().Where(e => e.Message.StartsWith("Empty"));

        Action act2 = () => customer.GreetAndCombineNames("", "Smith");
        act2
          .Should().Throw<ArgumentException>()
          .WithMessage("Empty*");

        //Should not thrown an exception:
        Action act3 = () => customer.GreetAndCombineNames("John", "Smith");
        act3.Should().NotThrow();
    }

    [Fact]
    public void CustomerType_CreateCcustomerWithLessThan100Order_ReturnBasicCustomer()
    {
        customer.OrderTotal = 10;
        var result = customer.GetCustomerDetails();
        Assert.IsType<BasicCustomer>(result);
    }

    [Fact]
    public void CustomerType_CreateCcustomerWithMoreThan100Order_ReturnPlatinumCustomer()
    {
        customer.OrderTotal = 200;
        var result = customer.GetCustomerDetails();
        Assert.IsType<PlatinumCustomer>(result);
    }
}
