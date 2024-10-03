namespace _01_2_BankOCRLibrary
{
    public static class BankOcrHelpers
    {
        public static void GetAccountNumbersFromFile(string fileNameOrPath)
        {
            if (!FileExists(fileNameOrPath))
                throw new ArgumentException($"File does not exist");

            string[] lines = File.ReadAllText(fileNameOrPath).Split('\n');

            if (!FileLineStructureIsValid(lines))
                throw new InvalidDataException($"File line structure is not valid");
        }

        private static bool FileExists(string fileNameOrPath)
        {
            if (!Path.IsPathRooted(fileNameOrPath))
                fileNameOrPath = Path.GetFullPath(fileNameOrPath);

            return File.Exists(fileNameOrPath);
        }

        private static bool FileLineStructureIsValid(string[] lines) => (lines.Length + 1) % 4 == 0;
    }
}