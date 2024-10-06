using CalLibrary;
using FluentAssertions;

namespace CalTests
{
    [TestClass]
    public class CalTests
    {
        [DataTestMethod]
        [DataRow(13, 2014)]
        [DataRow(-1, 2014)]
        [DataRow(0, 2014)]
        [DataRow(1, -17)]
        [DataRow(4, 0)]
        [DataRow(11, 10000)]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_arguments(int month, int year)
        {
            CalHelpers.GetCalendar(month, year);
        }

        [DataTestMethod]
        [DataRow(2, 2014, "    February 2014")]
        [DataRow(3, 2020, "     March 2020")]
        [DataRow(5, 2022, "      May 2022")]
        [DataRow(3, 12, "      March 12")]
        public void Test_valid_arguments_and_correct_title(int month, int year, string title)
        {
            string[] calendar = CalHelpers.GetCalendar(month, year).ToArray();
            calendar.Should().NotBeEmpty();
            calendar[0].Should().Be(title);
        }

        [DataTestMethod]
        [DataRow(2, 2014, "Invalid")]
        [DataRow(2, 2014, "Bummer")]
        [DataRow(2, 2014, "Another")]
        [DataRow(2, 2014, "Weekday")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_start_week_day_three_arguments(int month, int year, string startWeekday)
        {
            CalHelpers.GetCalendar(month, year, startWeekday).ToArray();
        }

        [DataTestMethod]
        [DataRow("Invalid")]
        [DataRow("Bummer")]
        [DataRow("Another")]
        [DataRow("Weekday")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_start_week_day_one_argument(string startWeekday)
        {
            CalHelpers.GetCalendar(startWeekday).ToArray();
        }

        [DataTestMethod]
        [DataRow(2, 2014, "Monday", "Mo Tu We Th Fr Sa Su")]
        [DataRow(2, 2014, "Tuesday", "Tu We Th Fr Sa Su Mo")]
        [DataRow(2, 2014, "Wednesday", "We Th Fr Sa Su Mo Tu")]
        [DataRow(2, 2014, "Thursday", "Th Fr Sa Su Mo Tu We")]
        [DataRow(2, 2014, "Friday", "Fr Sa Su Mo Tu We Th")]
        [DataRow(2, 2014, "Saturday", "Sa Su Mo Tu We Th Fr")]
        [DataRow(2, 2014, "Sunday", "Su Mo Tu We Th Fr Sa")]
        public void Test_valid_arguments_and_correct_header(int month, int year, string startWeekday, string header)
        {
            string[] calendar = CalHelpers.GetCalendar(month, year, startWeekday).ToArray();
            calendar.Length.Should().BeGreaterThan(1);
            calendar[1].Should().Be(header);
        }

        [TestMethod]
        public void Test_example()
        {
            string[] expected = [
                "    February 2014",
                "Su Mo Tu We Th Fr Sa",
                "                   1",
                " 2  3  4  5  6  7  8",
                " 9 10 11 12 13 14 15",
                "16 17 18 19 20 21 22",
                "23 24 25 26 27 28",
                ];
            string[] calendar = CalHelpers.GetCalendar(2, 2014).ToArray();
            calendar.Should().BeEquivalentTo(expected);
        }
    }
}