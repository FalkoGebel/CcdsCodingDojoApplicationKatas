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
            string[] result = sut.AddBookEntry([]).ToArray();

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
            string[] result = sut.AddBookEntry(arguments).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}