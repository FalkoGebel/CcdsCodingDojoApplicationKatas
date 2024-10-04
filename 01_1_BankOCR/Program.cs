using _01_2_BankOCRLibrary;

namespace _01_1_BankOCR
{
    public class Program
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
                foreach (string line in BankOcrHelpers.GetAccountNumbersFromFile(args[0]))
                    Console.WriteLine(line);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            catch (InvalidDataException ide)
            {
                Console.WriteLine(ide.Message);
            }
        }
    }
}