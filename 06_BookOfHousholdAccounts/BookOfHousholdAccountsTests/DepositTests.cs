using BookOfHousholdAccountsLibrary;
using FluentAssertions;

namespace BookOfHousholdAccountsTests
{
    [TestClass]
    public class DepositTests
    {
        private readonly BookOfHousholdAccountsBook sut = new(false);

        [DataTestMethod]
        [DataRow(new string[] { "deposit", "400" })]
        [DataRow(new string[] { "deposit", "500,20" })]
        [DataRow(new string[] { "deposit", "1111,11" })]
        [DataRow(new string[] { "deposit", "5000,12" })]
        public void Valid_deposit_without_date(string[] arguments)
        {
            // Act
            string[] result = sut.ProcessInput(arguments, out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo([]);
        }

        [DataTestMethod]
        [DataRow(false, "400")]
        [DataRow(true, "500,20")]
        [DataRow(false, "1111,11")]
        [DataRow(true, "5000,12")]
        public void Valid_deposit_with_date(bool future, string amount)
        {
            // Act
            DateTime dateTime = DateTime.Now.AddDays(future ? 5 : 0);
            string[] result = sut.ProcessInput(["deposit", dateTime.ToString("d"), amount], out bool _).ToArray();

            // Assert
            result.Should().BeEquivalentTo([]);
        }
    }
}