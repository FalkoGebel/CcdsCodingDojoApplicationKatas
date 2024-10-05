namespace BankOcrLibrary
{
    public static class BankOcrHelpers
    {
        public static string[] GetAccountNumbersFromFile(string fileNameOrPath)
        {
            if (!FileExists(fileNameOrPath))
                throw new ArgumentException($"File does not exist");

            string[] lines = File.ReadAllText(fileNameOrPath).Split('\n');

            if (!FileLineStructureIsValid(lines))
                throw new InvalidDataException($"File line structure is not valid");

            return GetAccountNumbersFromLines(lines);
        }

        private static string[] GetAccountNumbersFromLines(IEnumerable<string> lines)
        {
            List<string> output = [],
                         numberLines = lines.Where((x, i) => (i + 1) % 4 != 0).ToList();

            for (int i = 0; i < numberLines.Count; i += 3)
                output.Add(GetAccountNumberFromThreeLines(numberLines.Skip(i).Take(3)));

            return [.. output];
        }

        private static string GetAccountNumberFromThreeLines(IEnumerable<string> lines)
        {
            Dictionary<string, int> mapping = new() { { " _ | ||_|", 0 }, { "     |  |", 1 }, { " _  _||_ ", 2 },
                                                      { " _  _| _|", 3 }, { "   |_|  |", 4 }, { " _ |_  _|", 5 },
                                                      { " _ |_ |_|", 6 }, { " _   |  |", 7 }, { " _ |_||_|", 8 },
                                                      { " _ |_| _|", 9 } };

            string output = string.Empty,
                   firstLine = lines.First();

            if ((firstLine.Length + 1) % 4 == 0 &&
                lines.All(l => l.Length == firstLine.Length) &&
                lines.All(l => l.Where((x, i) => (i + 1) % 4 == 0).All(c => c == ' ')))
            {
                List<string> numberLines = lines.Select(l => string.Concat(l.Where((c, i) => (i + 1) % 4 != 0))).ToList();
                firstLine = numberLines.First();

                for (int i = 0; i < firstLine.Length; i += 3)
                {
                    string numberChars = string.Concat(numberLines.Select(l => string.Concat(l.Skip(i).Take(3))));

                    if (mapping.TryGetValue(numberChars, out int value))
                    {
                        output += value.ToString();
                    }
                    else
                    {
                        output = string.Empty;
                        break;
                    }
                }
            }

            return output == string.Empty ? "Error in data" : output;
        }

        private static bool FileExists(string fileNameOrPath)
        {
            if (!Path.IsPathRooted(fileNameOrPath))
                fileNameOrPath = Path.GetFullPath(fileNameOrPath);

            return File.Exists(fileNameOrPath);
        }

        private static bool FileLineStructureIsValid(string[] lines)
            => (lines.Length + 1) % 4 == 0 && lines.Where((x, i) => (i + 1) % 4 == 0).All(l => l == string.Empty);
    }
}