using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest;

public class GradingCalculatorXUnitTests
{
    private GradingCalculator gradingCalculator;
    public GradingCalculatorXUnitTests()
    {
        gradingCalculator = new GradingCalculator();
    }

    [Theory]
    [InlineData(95, 90, "A")]
    [InlineData(85, 90, "B")]
    [InlineData(65, 90, "C")]
    [InlineData(95, 65, "B")]
    [InlineData(95, 55, "F")]
    [InlineData(65, 55, "F")]
    [InlineData(50, 90, "F")]
    public void GradeCalc_InputScoreXAttendX_ReturnX(int score, int attendance, string expectedResult)
    {
        gradingCalculator.Score = score;
        gradingCalculator.AttendancePercentage = attendance;
        var result = gradingCalculator.GetGrade();
        Assert.Equal(expectedResult, result);
    }

}
