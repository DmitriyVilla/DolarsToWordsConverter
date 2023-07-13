using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DollarsToWords
{
    public class Converter
    {

        private static string[] units = {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
        "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static string[] tens = {
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };


        public static decimal ConvertStringToDecimal(string amountString, ref string error)
        {
            string cleanedAmountString = amountString.Replace(" ", "").Replace(",", ".");

            if (int.TryParse(cleanedAmountString, out int intValue))
            {
                decimal amount = Convert.ToDecimal(intValue);

                // Überprüfen, ob die Zahl negativ ist
                if (amount < 0)
                {
                    error = "Error:The amount must be greater than or equal to 0.";
                    return 0; 
                }

                // Überprüfen, ob die Zahl größer als 999999999 ist
                if (amount > 999999999.99m)
                {
                    error = "Error:The amount must be less than or equal to 999999999.99.";
                    return 0; 
                }

                return amount;
            }
            else if (decimal.TryParse(cleanedAmountString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal decimalValue))
            {
                // Überprüfen, ob die Zahl kleiner als 0 oder größer als 999999999,99 ist
                if (decimalValue < 0 || decimalValue > 999999999.99m)
                {
                    error = "Error:The amount must be between 0 and 999999999.99.";
                    return 0; 
                }

                return decimalValue;
            }
            else
            {
                error = "Error:Illegal format for amount string, amount must be a number.";
                return 0; 
            }
        }


        public static string ConvertDollarsToWords(decimal amount, ref string error)
        {
            string result = string.Empty;

            //// check diapason
            //if (amount < 0 || amount > 999999999.99m)
            //{
            //    error = string.IsNullOrEmpty(error) ? "The amount must be between 0 and 999999999.99." : error;

            //    return result;
            //    //throw new ArgumentOutOfRangeException("amount", "The amount must be between 0 and 999999999.99.");
            //}

            int dollars = (int)Math.Floor(amount);
            int cents = (int)((amount - dollars) * 100);

            string dollarsInWords = ConvertNumberToWords(dollars);
            string centsInWords = ConvertNumberToWords(cents);

            result = dollarsInWords + ((dollars == 1) ? " dollar" : " dollars");

            if (cents > 0)
            {
                result += " and " + centsInWords + ((cents == 1) ? " cent" : " cents");
            }

            return result;
        }

        private static string ConvertNumberToWords(int number)
        {
            if (number < 20)
            {
                return units[number];
            }

            if (number < 100)
            {
                return tens[number / 10] + ((number % 10 > 0) ? "-" + units[number % 10] : "");
            }

            if (number < 1000)
            {
                return units[number / 100] + " hundred" + ((number % 100 > 0) ? " " + ConvertNumberToWords(number % 100) : "");
            }

            if (number < 1000000)
            {
                return ConvertNumberToWords(number / 1000) + " thousand" + ((number % 1000 > 0) ? " " + ConvertNumberToWords(number % 1000) : "");
            }

            if (number < 1000000000)
            {
                return ConvertNumberToWords(number / 1000000) + " million" + ((number % 1000000 > 0) ? " " + ConvertNumberToWords(number % 1000000) : "");
            }

            throw new ArgumentOutOfRangeException("number", "The number is too large to convert to words.");
        }
    }
}
