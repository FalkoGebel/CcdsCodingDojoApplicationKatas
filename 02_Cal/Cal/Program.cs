using CalLibrary;

namespace Cal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] calendar = [];

                if (args.Length == 0)
                {
                    calendar = CalHelpers.GetCalendar().ToArray();
                }
                else if (args.Length == 1)
                {
                    calendar = CalHelpers.GetCalendar(args[0]).ToArray();
                }
                else if (args.Length == 2 || args.Length == 3)
                {
                    if (!int.TryParse(args[0], out int month))
                    {
                        Console.WriteLine("Invalid first parameter - has to be an integer");
                        return;
                    }
                    if (!int.TryParse(args[1], out int year))
                    {
                        Console.WriteLine("Invalid second parameter - has to be an integer");
                        return;
                    }

                    if (args.Length == 2)
                        calendar = CalHelpers.GetCalendar(month, year).ToArray();
                    else
                        calendar = CalHelpers.GetCalendar(month, year, args[2]).ToArray();
                }
                else
                {
                    Console.WriteLine("Invalid number of parameters - has to between 0 and 3");
                    return;
                }

                foreach (string line in calendar)
                    Console.WriteLine(line);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}