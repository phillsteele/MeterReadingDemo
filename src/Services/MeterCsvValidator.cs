using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    public class MeterCsvValidator : IMeterCsvValidator
    {
        public bool ValidateHeaders(string[] validHeaders, string[] headerTokens)
        {
            if (validHeaders.Length != headerTokens.Length)
                return false;

            // Each valid header must exist in the header tokens

            // We don't care about the specific order, only that they contain the same tokens
            foreach (string validHeader in validHeaders)
            {
                if (!headerTokens.Contains(validHeader))
                    return false;
            }

            return true;
        }

        public bool TryParseAccountId(string token, out int accountId)
        {
            accountId = 0;

            // First token must be a number.
            if (!int.TryParse(token, out int parsedAccountId))
                return false;

            // It must be a positive int
            if (parsedAccountId < 0)
                return false;

            accountId = parsedAccountId;

            return true;
        }

        public bool TryParseMeterReadingDateTime(string token, out DateTime meterReadingDateTime)
        {
            meterReadingDateTime = DateTime.MinValue;

            if (!DateTime.TryParse(token, out DateTime parsedMeterReadingDateTime))
                return false;

            meterReadingDateTime = parsedMeterReadingDateTime;

            return true;
        }

        public bool TryParseMeterReading(string token, out string meterReading)
        {
            meterReading = String.Empty;

            if (!Regex.IsMatch(token, @"^\d{5}$"))
                return false;

            meterReading = token;

            return true;
        }


    }
}
