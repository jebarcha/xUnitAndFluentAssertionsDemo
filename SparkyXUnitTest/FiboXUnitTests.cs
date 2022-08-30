using FluentAssertions;

namespace SparkyXUnitTest
{
    public class FiboXUnitTests
    {
        //Input(Range): 1
        //-List is not empty
        //-List is ordered
        //-List should match {0}
        [Fact]
        public void FiboChecker_Input1_Returns0()
        {
            List<int> expected = new() { 0 };

            Fibo fibo = new();
            fibo.Range = 1;

            List<int> result = fibo.GetFiboSeries();
            result.Should().NotBeEmpty()
                .And.BeInAscendingOrder()
                .And.BeEquivalentTo(expected);
        }

        //Input(Range): 6
        //-List contains 3 number
        //-List count is 6
        //-List does not contain 4 number
        //-List should match {0,1,1,2,3,5}
        [Fact]
        public void FiboChecker_Input6_ReturnsFiboSeries()
        {
            List<int> expected = new() { 0, 1, 1, 2, 3, 5 };

            Fibo fibo = new();
            fibo.Range = 6;

            List<int> result = fibo.GetFiboSeries();
            result.Should().NotBeEmpty()
                .And.HaveCount(6)
                .And.NotContain(4)
                .And.BeInAscendingOrder()
                .And.BeEquivalentTo(expected);
        }
    }
}
