using _01_2_BankOCRLibrary;

namespace _01_3_BankOCRTests
{
    [TestClass]
    public class HelpersTests
    {
        private static string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BankOCRKata/";
        private static string _validFile1 = "file1.txt";
        private static string _validFile2 = "file2.txt";
        private static string _validFile3 = "file3.txt";
        private static string _invalidFile1 = "file4.txt";
        private static string _invalidFile2 = "file5.txt";
        private static string _invalidFile3 = "file6.txt";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Directory.CreateDirectory(_folderpath);
            File.WriteAllText(_folderpath + _validFile1, "    _  _     _  _  _  _  _ \n  | _| _||_||_ |_   ||_||_|\n  ||_  _|  | _||_|  ||_| _|");
            File.WriteAllText(_folderpath + _validFile2, "    _  _  _  _  _  _     _ \n|_||_|| || ||_   |  |  ||_ \n  | _||_||_||_|  |  |  | _|");
            File.WriteAllText(_folderpath + _validFile3, "    _  _     _  _  _  _  _ \n  | _| _||_||_ |_   ||_||_|\n  ||_  _|  | _||_|  ||_| _|\n\n    _  _  _  _  _  _     _ \n|_||_|| || ||_   |  |  ||_ \n  | _||_||_||_|  |  |  | _|");
            File.WriteAllText(_folderpath + _invalidFile1, "");
            File.WriteAllText(_folderpath + _invalidFile2, "    _  _     _  _  _  _  _ \n");
            File.WriteAllText(_folderpath + _invalidFile3, "    _  _     _  _  _  _  _ \n  | _| _||_||_ |_   ||_||_|\n  ||_  _|  | _||_|  ||_| _|\n\n    _  _  _  _  _  _     _ \n");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(@"C:\file.txt")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Invalid_File_Names(string filePath)
        {
            BankOcrHelpers.GetAccountNumbersFromFile(filePath);
        }

        [TestMethod]
        public void Test_Valid_Files()
        {
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile1);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile2);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _validFile3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Test_Ivnalid_Files()
        {
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _invalidFile1);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _invalidFile2);
            BankOcrHelpers.GetAccountNumbersFromFile(_folderpath + _invalidFile3);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(_folderpath, true);
        }
    }
}