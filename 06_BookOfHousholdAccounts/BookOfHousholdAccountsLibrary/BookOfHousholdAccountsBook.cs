using Newtonsoft.Json;

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

        public IEnumerable<string> AddBookEntry(string[] arguments)
        {
            if (arguments.Length == 0)
                return ["No arguments given. Use \"-?\" as parameter to show help."];

            List<string> output = [];

            // TODO - Show help

            // TODO - Show overview

            // TODO - Process deposit with date
            if (arguments[0].Equals("deposit", StringComparison.CurrentCultureIgnoreCase))
            {
                decimal amount = Convert.ToDecimal(arguments[1]);

                if (amount > 0)
                {

                    _bookEntries.Add(new BookEntryModel
                    {
                        EntryType = BookEntryType.Deposit,
                        PostingDate = DateTime.Now,
                        Category = string.Empty,
                        MemoText = string.Empty,
                        Amount = amount
                    });

                    decimal cashBalance = _bookEntries.Where(be => be.PostingDate.Date <= DateTime.Now.Date && be.EntryType == BookEntryType.Deposit)
                                                      .Select(be => be.Amount)
                                                      .Sum() -
                                          _bookEntries.Where(be => be.PostingDate.Date <= DateTime.Now.Date && be.EntryType == BookEntryType.Spending)
                                                      .Select(be => be.Amount)
                                                      .Sum();
                    output.Add($"Cash balance: {cashBalance:F2} EUR");
                }
                else
                {
                    output.Add("Invalid amount. Amount has to be positive.");
                }

                SaveBookEntriesToFile();
            }

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