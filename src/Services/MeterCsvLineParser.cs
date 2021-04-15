using MeterReader.Model;
using System;

namespace MeterReader.Services
{
    public class MeterCsvLineParser : IMeterCsvLineParser
    {
        private readonly IMeterCsvValidator _meterCsvValidator;

        public MeterCsvLineParser(IMeterCsvValidator meterCsvValidator) => _meterCsvValidator = meterCsvValidator;

        public bool TryParseMeterLine(string csvMeterLine, out MeterReading successfulReading)
        {
            successfulReading = new MeterReading();

            // Make an assumption that the order of the headers is: AccountId,MeterReadingDateTime,MeterReadValue
            // Additional code would be required if that assumption could not be relied upon.  We would need
            // to determine the position of each header and process accordingly.

            string[] tokens = csvMeterLine.Split(',');

            // We expect exactly 3 entries
            if (tokens.Length != 3)
                return false;

            if (!_meterCsvValidator.TryParseAccountId(tokens[0], out int accountId))
                return false;

            // Seconds token must be a valid date time
            if (!_meterCsvValidator.TryParseMeterReadingDateTime(tokens[1], out DateTime meterReadingDateTime))
                return false;

            // Last token must be a string in the format of NNNNN
            if (!_meterCsvValidator.TryParseMeterReading(tokens[2], out string meterReading))
                return false;

            successfulReading.AccountId = accountId;
            successfulReading.MeterReadingDateTime = meterReadingDateTime;
            successfulReading.MeterReadValue = meterReading;

            return true;
        }
    }
}
