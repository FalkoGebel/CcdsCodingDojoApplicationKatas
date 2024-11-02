using BookOfHousholdAccountsLibrary;

namespace BookOfHousholdAccounts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookOfHousholdAccountsBook BookOfHousholdAccountsBook = new();
            foreach (string result in BookOfHousholdAccountsBook.AddBookEntry(args))
                Console.WriteLine(result);
        }
    }
}