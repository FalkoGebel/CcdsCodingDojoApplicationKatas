namespace CalLibrary
{
    public static class CalHelpers
    {
        public static IEnumerable<string> GetCalendar()
        {
            DateTime dateTime = DateTime.Now;
            return GetCalendar(dateTime.Month, dateTime.Year);
        }

        public static IEnumerable<string> GetCalendar(string startWeekday)
        {
            DateTime dateTime = DateTime.Now;
            return GetCalendar(dateTime.Month, dateTime.Year, startWeekday);
        }

        public static IEnumerable<string> GetCalendar(int month, int year)
        {
            return GetCalendar(month, year, "Sunday");
        }

        public static IEnumerable<string> GetCalendar(int month, int year, string startWeekday)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Invalid month - has to be between 1 and 12");

            if (year < 1 || year > 9999)
                throw new ArgumentException("Invalid year - has to be between 1 and 9999");

            List<string> output = [];

            DateOnly firstDayOfMonth = new(year, month, 1);

            output.AddTitle(firstDayOfMonth);

            int startIndex = output.AddHeader(startWeekday);

            if (startIndex < 0)
                throw new ArgumentException($"\"{startWeekday}\" is not a valid weekday to start (valid: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday (default))");

            DateOnly currentDay = firstDayOfMonth;
            startIndex = (int)currentDay.DayOfWeek - startIndex;

            while (currentDay.Month == firstDayOfMonth.Month)
            {
                currentDay = output.AddWeek(currentDay, startIndex);
                startIndex = 0;
            }

            return output;
        }

        private static DateOnly AddWeek(this List<string> calendar, DateOnly currentDate, int startIndex)
        {
            List<string> week = [];
            int month = currentDate.Month;

            if (startIndex < 0)
                startIndex += 7;

            if (currentDate.Day == 1)
                week = [.. week, .. Enumerable.Range(0, startIndex).Select(i => "  ").ToList()];

            while (startIndex < 7)
            {
                week.Add(currentDate.Day.ToString().PadLeft(2, ' '));
                startIndex++;
                currentDate = currentDate.AddDays(1);
                if (currentDate.Month > month)
                    break;
            }

            calendar.Add(string.Join(' ', week));
            return currentDate;
        }

        private static void AddTitle(this List<string> calendar, DateOnly firstDayOfMonth)
        {
            string title = firstDayOfMonth.Month switch
            {
                1 => "January",
                2 => "February",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                _ => "December",
            };
            title += $" {firstDayOfMonth.Year}";
            title = new string(' ', (int)Math.Ceiling((20.0 - title.Length) / 2)) + title;

            calendar.Add(title);
        }

        private static int AddHeader(this List<string> calendar, string startWeekday)
        {
            string[] weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            int idxWeekDay = Array.IndexOf(weekdays, startWeekday);

            if (idxWeekDay < 0)
                return idxWeekDay;

            string[] weekdaysShort = weekdays.Select(wd => wd[..2]).ToArray();
            calendar.Add(string.Join(' ', weekdaysShort.Skip(idxWeekDay).Concat(weekdaysShort.Take(idxWeekDay))));

            return idxWeekDay;
        }
    }
}