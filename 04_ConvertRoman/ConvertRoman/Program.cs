using ConvertRomanLibrary;

namespace ConvertRoman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of start parameters. You need to start the program with one parameter (roman number, arabic number or '-f=\"<FILEPATH>\"' to check a text file with multiple inputs).");
                return;
            }

            try
            {
                foreach (string result in ConvertRomanHelpers.GetResults(args[0]))
                    Console.WriteLine(result);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}
