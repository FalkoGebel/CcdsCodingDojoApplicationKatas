using Newtonsoft.Json;
using System.Globalization;

namespace BookOfHousholdAccountsLibrary
{
    public class BookOfHousholdAccountsBook
    {
        const string _fileName = "boha.txt";
        readonly bool _useFileFunctions;
        List<BookEntryModel> _bookEntries = [];

        public BookOfHousholdAccountsBook(bool useFileFunctions = true)
        {
            _useFileFunctions = useFileFunctions;
            LoadBookEntriesFromFile();
        }

        private void LoadBookEntriesFromFile()
        {
            if (!_useFileFunctions)
                return;

            _bookEntries = ReadFromJsonFile<List<BookEntryModel>>(_fileName);
        }

        private void SaveBookEntriesToFile()
        {
            if (!_useFileFunctions)
                return;

            WriteToJsonFile(_fileName, _bookEntries);
        }

        private IEnumerable<string> AddDepositBookEntry(string[] parameters)
        {
            if (parameters.Length < 1 || parameters.Length > 2)
                return ["Invalid number of deposit arguments given. Use \"-?\" as parameter to show help."];

            DateTime dateTime = DateTime.Now;
            int amountIndex = 0;

            if (parameters.Length == 2)
            {
                try
                {
                    dateTime = DateTime.ParseExact(parameters[0], "d", CultureInfo.CurrentCulture);
                    amountIndex++;
                }
                catch (FormatException)
                {
                    return ["Invalid deposit date argument given. Use \"-?\" as parameter to show help."];
                }
            }

            decimal amount;
            try
            {
                amount = Convert.ToDecimal(parameters[amountIndex]);
            }
            catch (FormatException)
            {
                return ["Invalid deposit amount argument given. Use \"-?\" as parameter to show help."];
            }

            if (amount > 0)
            {

                _bookEntries.Add(new BookEntryModel
                {
                    EntryType = BookEntryType.Deposit,
                    PostingDate = dateTime,
                    Category = string.Empty,
                    MemoText = string.Empty,
                    Amount = amount
                });

                SaveBookEntriesToFile();
                return [GetCashBalance(DateTime.Now)];
            }
            else
            {
                return ["Invalid amount. Amount has to be positive."];
            }
        }

        private string GetCashBalance(DateTime dateTime)
        {
            decimal amount = _bookEntries.Where(be => be.PostingDate.Date <= dateTime && be.EntryType == BookEntryType.Deposit)
                                              .Select(be => be.Amount)
                                              .Sum() -
                             _bookEntries.Where(be => be.PostingDate.Date <= dateTime && be.EntryType == BookEntryType.Spending)
                                              .Select(be => be.Amount)
                                              .Sum(); ;
            return $"Cash balance: {amount:F2} EUR";
        }

        private IEnumerable<string> GetMonthOverview(string[] parameters)
        {
            if (parameters.Length != 0 && parameters.Length != 2)
                return ["Invalid number of overview arguments given. Use \"-?\" as parameter to show help."];

            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;

            if (parameters.Length == 2)
            {
                if (!int.TryParse(parameters[0], out var month) || month < 1 || month > 12)
                    return ["Invalid overview month argument given. Use \"-?\" as parameter to show help."];

                if (!int.TryParse(parameters[1], out var year))
                    return ["Invalid overview year argument given. Use \"-?\" as parameter to show help."];

                try
                {
                    firstOfMonth = new DateTime(year, month, 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return ["Invalid overview year argument given. Use \"-?\" as parameter to show help."];
                }
            }

            DateTime endOfMonth = firstOfMonth.AddMonths(1);
            endOfMonth = endOfMonth.AddDays(-1);

            LoadBookEntriesFromFile();

            List<string> output = [$"{firstOfMonth.ToString("Y")}",
                                   "------------------------",
                                   GetCashBalance(firstOfMonth)];

            foreach (BookEntryModel entry in _bookEntries.Where(be => be.PostingDate >= firstOfMonth && be.PostingDate <= endOfMonth))
                output.Add($"{(entry.EntryType == BookEntryType.Deposit ? "Deposit" : entry.Category)}: {entry.Amount:F2}");

            return output;
        }

        public IEnumerable<string> AddBookEntry(string[] arguments)
        {
            if (arguments.Length == 0)
                return ["No arguments given. Use \"-?\" as parameter to show help."];

            List<string> output = [];

            // TODO - Show help

            if (arguments[0].Equals("overview", StringComparison.CurrentCultureIgnoreCase))
                return GetMonthOverview(arguments[1..]);

            if (arguments[0].Equals("deposit", StringComparison.CurrentCultureIgnoreCase))
                return AddDepositBookEntry(arguments[1..]);

            // TODO - Process spending

            return output;
        }

        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter? writer = null;

            try
            {
                FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(fs);
                writer.Write(contentsToWriteToFile);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader? reader = null;

            try
            {
                FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                reader = new StreamReader(fs);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents) ?? new();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}