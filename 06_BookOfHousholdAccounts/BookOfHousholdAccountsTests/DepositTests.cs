using BookOfHousholdAccountsLibrary;
using FluentAssertions;

namespace BookOfHousholdAccountsTests
{
    [TestClass]
    public class DepositTests
    {
        private readonly BookOfHousholdAccountsBook sut = new(false);

        [DataTestMethod]
        [DataRow(new string[] { "deposit", "400" }, new string[] { "Cash balance: 400,00 EUR" })]
        [DataRow(new string[] { "deposit", "500,20" }, new string[] { "Cash balance: 500,20 EUR" })]
        [DataRow(new string[] { "deposit", "1111,11" }, new string[] { "Cash balance: 1111,11 EUR" })]
        [DataRow(new string[] { "deposit", "5000,12" }, new string[] { "Cash balance: 5000,12 EUR" })]
        public void Valid_deposit_without_date(string[] arguments, string[] expected)
        {
            // Act
            string[] result = sut.AddBookEntry(arguments).ToArray();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}