using BookOfHousholdAccountsLibrary;

namespace BookOfHousholdAccounts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookOfHousholdAccountsBook BookOfHousholdAccountsBook = new();
            string[] result = BookOfHousholdAccountsBook.ProcessInput(args, out bool secondCall).ToArray();

            if (secondCall)
            {
                Console.Write(result[0]);
                var input = Console.ReadLine();
                if (input != null)
                    _ = BookOfHousholdAccountsBook.ProcessInput(input.Split(), out _);
            }
            else
            {
                foreach (string line in result)
                    Console.WriteLine(line);
            }
        }
    }
}