using Bongo.Models.ModelValidations;

namespace Bongo.Models;
public class DateInFutureAttributeTest
{
    [Theory]
    [InlineData(100, true)]
    [InlineData(-100, false)]
    [InlineData(0, false)]
    public void DateValidator_InputExpectedDateRange_DateValidity(int addSeconds, bool expected)
    {
        DateInFutureAttribute dateInFutureAttribute = new(() => DateTime.Now);

        bool result = dateInFutureAttribute.IsValid(DateTime.Now.AddSeconds(addSeconds));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateValidator_AnyDate_ReturnErrorMEssage()
    {
        var result = new DateInFutureAttribute();
        Assert.Equal("Date must be in the future", result.ErrorMessage);
    }
}

