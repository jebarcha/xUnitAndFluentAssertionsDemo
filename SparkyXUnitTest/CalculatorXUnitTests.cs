using FluentAssertions;
using System.Collections.Generic;

namespace SparkyXUnitTest;
public class CalculatorXUnitTests
{
    private Calculator calc;
    public CalculatorXUnitTests()
    {
        calc = new();
    }

    [Fact]
    public void AddNumbers_InputTwoInt_GetCorrectAddition()
    {
        //Arrange
        //Calculator calc = new();

        //Act
        int result = calc.AddNumbers(10, 20);

        //Assert
        Assert.Equal(30, result);
    }

    [Theory]
    [InlineData(10, false)]
    [InlineData(9, true)]
    public void IsOddNumber_InputNumber_ReturnBool(int num, bool expected)
    {
        bool result = calc.IsOddNmber(num);
        Assert.Equal(expected, result);
    }

    //[Fact]
    //public void IsOddNumber_InputOddNumber_ReturnTrue()
    //{
    //    bool result = calc.IsOddNmber(9);
    //    Assert.True(result);
    //}

    //[Theory]
    //[InlineData(5.4, 10.5)]   //15.9
    //[InlineData(5.43, 10.53)] //15.96
    ////[InlineData(5.49, 10.59)] //16.08
    //public void AddNumbersDouble_InputTwoDouble_GetCorrectAddition(double a, double b)
    //{
    //    //Act
    //    double result = calc.AddNumbersDouble(a, b);

    //    //Assert
    //    Assert.Equal(15.9, result, 1);
    //}

    [Fact]
    public void OddRanger_InputMinAndMaxRange_ReturnsValidOddNumberRange()
    {
        List<int> expected = new() { 5, 7, 9 };  //5-10
        List<int> result = calc.GetOddRange(5, 10);

        //Assert.Equal(expected, result);
        //Assert.Contains(7, result);
        Assert.DoesNotContain(1, result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);

        //Assert.Collection(result,
        //                item => Assert.Equal(5, item),
        //                item => Assert.Equal(7, item),
        //                item => Assert.Equal(9, item)
        //            );
    }

    [Fact]
    public void FluentAssertions_OddRanger_InputMinAndMaxRange_ReturnsValidOddNumberRange()
    {
        List<int> expected = new() { 5, 7, 9 };  //5-10
        List<int> result = calc.GetOddRange(5, 10);

        // Using FluentAssertions
        result.Should().NotBeEmpty()
               .And.HaveCount(3, "Because we are using a 3 sample")
               .And.ContainInOrder(new[] { 5, 7, 9 })
               .And.ContainItemsAssignableTo<int>();

        result.Should().Equal(expected);
        result.Should().Equal(5, 7, 9);
        result.Should().HaveCount(c => c >= 3)
            .And.OnlyHaveUniqueItems();
        result.Should().HaveCountGreaterThan(2);
        result.Should().StartWith(5);
        result.Should().EndWith(9);
        result.Should().StartWith(new[] { 5, 7 });
        result.Should().BeInAscendingOrder();
    }

}
