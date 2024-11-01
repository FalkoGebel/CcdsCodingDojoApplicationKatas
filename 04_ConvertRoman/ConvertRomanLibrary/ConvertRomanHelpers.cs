
namespace ConvertRomanLibrary
{
    public class ConvertRomanHelpers
    {
        readonly private static string[] _romanNumerals = ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"];
        readonly private static int[] _romanValues = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];

        public static IEnumerable<string> GetResults(string argument)
        {
            List<string> output = [];
            argument = argument.Replace("\"", "");

            if (argument.StartsWith("-f="))
            {
                string path = argument[3..];

                try
                {
                    StreamReader sr = new(path);
                    string? line;

                    while (true)
                    {
                        line = sr.ReadLine();

                        if (line == null)
                            break;

                        output.Add(GetConvertedNumber(line));
                    }
                }
                catch (Exception e)
                {
                    output.Add(e.Message);
                }
            }
            else
            {
                output.Add(GetConvertedNumber(argument));
            }

            return output;
        }

        private static string GetConvertedNumber(string input)
        {
            if (input.All(c => char.IsDigit(c)))
            {
                try
                {
                    return GetRomanNumeralFromDecimal(input);
                }
                catch (ArgumentException ae)
                {
                    return ae.Message;
                }
            }
            else
            {
                try
                {
                    return GetDecimalFromRomanNumeral(input).ToString();
                }
                catch (ArgumentException ae)
                {
                    return ae.Message;
                }
            }
        }

        private static int GetDecimalFromRomanNumeral(string romanNumeral)
        {
            int output = 0;
            string numeral = romanNumeral;

            for (int i = 0; i < _romanNumerals.Length; i++)
            {
                int counter = 0,
                    skip = 0;

                while (numeral.StartsWith(_romanNumerals[i]))
                {
                    if (_romanNumerals[i].Length == 2 && counter > 0)
                        ThrowInvalidRomanNumeralException(romanNumeral);

                    output += _romanValues[i];
                    numeral = numeral[_romanNumerals[i].Length..];
                    counter++;
                    skip = _romanNumerals[i] == "IX" || _romanNumerals[i] == "XC" || _romanNumerals[i] == "CM"
                        ? 3
                        : _romanNumerals[i].Length == 2 ? 1 : 0;
                }

                if (counter > 3)
                    ThrowInvalidRomanNumeralException(romanNumeral);

                i += skip;
            }

            if (numeral != string.Empty)
                ThrowInvalidRomanNumeralException(romanNumeral);

            if (output > 3000)
                throw new ArgumentException($"Only roman numerals between \"I\" and \"MMM\" allowed");

            return output;
        }

        private static void ThrowInvalidRomanNumeralException(string romanNumeral)
        {
            throw new ArgumentException($"\"{romanNumeral}\" is not a valid roman numeral");
        }

        private static string GetRomanNumeralFromDecimal(string arabicNumber)
        {
            if (int.TryParse(arabicNumber, out int number))
            {
                if (number < 1 || number > 3000)
                    throw new ArgumentException("Invalid input - integer not between 1 and 3000");

                string output = string.Empty;

                for (int i = 0; i < _romanValues.Length; i++)
                {
                    while (number >= _romanValues[i])
                    {
                        output += _romanNumerals[i];
                        number -= _romanValues[i];
                    }

                    if (number == 0)
                        break;
                }

                return output;
            }
            else
            {
                throw new ArgumentException("Invalid input - not an integer");
            }
        }
    }
}
