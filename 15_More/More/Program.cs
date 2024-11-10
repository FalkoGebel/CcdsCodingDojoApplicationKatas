using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace More
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Invalid number of parameters - the following variants are possible:");
                Console.WriteLine("simple file name only:\t\t\tmore textfile.txt");
                Console.WriteLine("complex file name only:\t\t\tmore \"D:\\folder\\text file.txt\"");
                Console.WriteLine("simple file name and page length:\tmore textfile.txt 10");
                Console.WriteLine("complex file name and page length:\tmore \"D:\\folder\\text file.txt\" 10");
                return;
            }

            int pageLength = 20;

            if (args.Length == 2)
            {
                if (!int.TryParse(args[1], out pageLength) || pageLength <= 0)
                    Console.WriteLine("Invalid page length - has to be a positive integer");
            }

            string filePath = args[0].Replace("\"", "");
            List<string> lines = new List<string>();

            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);

                string line = sr.ReadLine();

                while (line != null)
                {
                    lines.Add(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }

            int numberOfPages = lines.Count / pageLength;

            if (numberOfPages * pageLength < lines.Count)
                numberOfPages++;

            int counter = 1;

            while (counter <= numberOfPages)
            {
                Console.WriteLine($"--- Page {counter} of {numberOfPages} ---");

                foreach (string line in lines.Skip((counter - 1) * pageLength).Take(pageLength))
                    Console.WriteLine(line);

                if (counter < numberOfPages)
                {
                    Console.WriteLine("--- Press any key to show next page. Exit by pressing [Esc] ---");

                    ConsoleKeyInfo info = Console.ReadKey();

                    if (info.Key == ConsoleKey.Escape)
                        return;
                }

                counter++;
            }
        }
    }
}