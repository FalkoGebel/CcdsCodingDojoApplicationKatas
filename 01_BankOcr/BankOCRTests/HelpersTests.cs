using BankOcrLibrary;
using FluentAssertions;

namespace BankOcrTests
{
    [TestClass]
    public class HelpersTests
    {
        private static readonly string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BankOCRKata/";
        private static readonly string _validFile1 = "valid1.txt";
        private static readonly string _validFile2 = "valid2.txt";
        private static readonly string _validFile3 = "valid3.txt";
        private static readonly string _validFile4 = "valid4.txt";
        private static readonly string _validFile5 = "valid5.txt";
        private static readonly string _invalidFile1 = "invalid1.txt";
        private static readonly string _invalidFile2 = "invalid2.txt";
        private static readonly string _invalidFile3 = "invalid3.txt";
        private static readonly string _invalidFile4 = "invalid4.txt";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Directory.CreateDirectory(_folderpath);
            File.WriteAllText(_folderpath + _validFile1, "     _   _       _   _   _   _   _ \n  |  _|  _| |_| |_  |_    | |_| |_|\n  | |_   _|   |  _| |_|   | |_|  _|");
            File.WriteAllText(_folderpath + _validFile2, "     _   _   _   _   _   _       _ \n|_| |_| | | | | |_    |   |   | |_ \n  |  _| |_| |_| |_|   |   |   |  _|");
            File.WriteAllText(_folderpath + _validFile3, "     _   _       _   _   _   _   _ \n  |  _|  _| |_| |_  |_    | |_| |_|\n  | |_   _|   |  _| |_|   | |_|  _|\n\n     _   _   _   _   _   _       _ \n|_| |_| | | | | |_    |   |   | |_ \n  |  _| |_| |_| |_|   |   |   |  _|");
            File.WriteAllText(_folderpath + _validFile4, "     _   _       _   _   _   _   _ \n  |  _|  _| |_| |_  |_    | |_| |_|\n  | |_   _|   |  _| |_|   | |_|  _|\n\n    _  _  _  _  _  _     _ \n|_||_|| || ||_   |  |  ||_ \n  | _||_||_||_|  |  |  | _|");
            File.WriteAllText(_folderpath + _validFile5, "     _   _       _   _   _   _   _ \n  |  _|  _| | | |_  |_    | |_| |_|\n  | |_   _|   |  _| |_|   | |_|  _|\n\n     _   _   _   _   _   _       _ \n|_| |_| | | | | |_    |   |   | |_ \n  |  _| |_| |_| |_|   |   |   |  _|");
            File.WriteAllText(_folderpath + _invalidFile1, "");
            File.WriteAllText(_folderpath + _invalidFile2, "    _  _     _  _  _  _  _ \n");
            File.WriteAllText(_folderpath + _invalidFile3, "    _  _     _  _  _  _  _ \n  | _| _||_||_ |_   ||_||_|\n  ||_  _|  | _||_|  ||_| _|\n\n    _  _  _  _  _  _     _ \n");
            File.WriteAllText(_folderpath + _invalidFile4, "    _  _     _  _  _  _  _ \n  | _| _||_||_ |_   ||_||_|\n  ||_  _|  | _||_|  ||_| _|\n \n    _  _  _  _  _  _     _ \n|_||_|| || ||_   |  |  ||_ \n  | _||_||_||_|  |  |  | _|");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(@"C:\file.txt")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_file_names(string filePath)
        {
            BankOcrHelpers.GetAccountNumbersFromFile(filePath);
        }

        [TestMethod]
        public void Test_valid_files()
        {
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile1);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile2);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile3);
        }

        [DataTestMethod]
        [DataRow("invalid1.txt")]
        [DataRow("invalid2.txt")]
        [DataRow("invalid3.txt")]
        [DataRow("invalid4.txt")]
        [ExpectedException(typeof(InvalidDataException))]
        public void Test_invalid_files(string fileName)
        {
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + fileName);
        }

        [DataTestMethod]
        [DataRow("valid1.txt", new string[] { "123456789" })]
        [DataRow("valid2.txt", new string[] { "490067715" })]
        [DataRow("valid3.txt", new string[] { "123456789", "490067715" })]
        [DataRow("valid4.txt", new string[] { "123456789", "Error in data" })]
        [DataRow("valid5.txt", new string[] { "Error in data", "490067715" })]
        public void Test_valid_files_and_correct_numbers(string fileName, string[] numbers)
        {
            string[] result = BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + fileName);
            result.Should().BeEquivalentTo(numbers);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(_folderpath, true);
        }
    }
}