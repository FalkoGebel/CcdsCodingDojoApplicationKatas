using BookOfHousholdAccountsLibrary;
using FluentAssertions;

namespace BookOfHousholdAccountsTests
{
    [TestClass]
    public class InvalidInputTests
    {
        private readonly BookOfHousholdAccountsBook sut = new(false);

        [TestMethod]
        public void Test_no_arguments()
        {
            // Arrange
            string[] expected = ["No arguments given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput([], out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["deposit", "-400"])]
        [DataRow(["deposit", "-1"])]
        public void Test_negative_amount(string[] arguments)
        {
            // Arrange
            string[] expected = ["Invalid amount. Amount has to be positive."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["deposit"])]
        [DataRow(["deposit", "11", "11", "11"])]
        [DataRow(["overview", "11"])]
        [DataRow(["overview", "11", "11", "11"])]
        public void Test_invalid_number_of_arguments(string[] arguments)
        {
            // Arrange
            string[] expected = [$"Invalid number of {arguments[0]} arguments given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["deposit", "", ""])]
        [DataRow(["deposit", "11", ""])]
        [DataRow(["deposit", "31.02.2020", ""])]
        public void Test_invalid_deposit_date_argument(string[] arguments)
        {
            // Arrange
            string[] expected = ["Invalid deposit date argument given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["deposit", "01.01.2020", "abc"])]
        [DataRow(["deposit", "abc"])]
        public void Test_invalid_deposit_amount_argument(string[] arguments)
        {
            // Arrange
            string[] expected = ["Invalid deposit amount argument given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["overview", "", ""])]
        [DataRow(["overview", "abc", ""])]
        [DataRow(["overview", "0", ""])]
        [DataRow(["overview", "13", ""])]
        public void Test_invalid_overview_month_argument(string[] arguments)
        {
            // Arrange
            string[] expected = ["Invalid overview month argument given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(["overview", "1", ""])]
        [DataRow(["overview", "3", "abc"])]
        [DataRow(["overview", "6", "-1"])]
        [DataRow(["overview", "11", "10000"])]
        public void Test_invalid_overview_year_argument(string[] arguments)
        {
            // Arrange
            string[] expected = ["Invalid overview year argument given. Use \"-?\" as parameter to show help."];

            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}