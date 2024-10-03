using _01_2_BankOCRLibrary;

namespace _01_1_BankOCR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No file name or file path given.");
                return;
            }

            try
            {
                BankOcrHelpers.GetAccountNumbersFromFile(args[0]);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}