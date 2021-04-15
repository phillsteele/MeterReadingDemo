using MeterReader.Model;
using System;
using System.Collections.Generic;

namespace MeterReader.Services
{
    public class MeterCsvReader : IMeterCsvReader
    {
        private static readonly string[] ValidHeaders = new string[] { "AccountId", "MeterReadingDateTime", "MeterReadValue" };

        private readonly IMeterCsvValidator _meterCsvValidator;
        private readonly IMeterCsvLineParser _meterCsvLineParser;

        public MeterCsvReader(
            IMeterCsvValidator meterCsvValidator,
            IMeterCsvLineParser meterCsvLineParser)
        {
            _meterCsvValidator = meterCsvValidator;
            _meterCsvLineParser = meterCsvLineParser;
        }

        public (List<MeterReading>, List<string>) ParseCsv(string rawCsv)
        {
            List<string> failedLines = new List<string>();
            List<MeterReading> successfulReadings = new List<MeterReading>();

            // If empty just return
            if (rawCsv == null || String.IsNullOrWhiteSpace(rawCsv))
                return (successfulReadings, failedLines);

            // Split by carriage returns / line feeds
            string[] rawLines = rawCsv.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Check, do we have a valid header?

            // Making an assumption that the first line of a CSV file will always contain the header
            // if it does not then the CSV is invalid.  If that assumption is incorrect then additional
            // checks will need to be made to locate the header in the list of strings.

            // Note that there will always be at least 1 line, the previous check for empty entries confirms this.
            string header = rawLines[0];

            string[] headerTokens = header.Split(',');

            if (!_meterCsvValidator.ValidateHeaders(ValidHeaders, headerTokens))
                return (successfulReadings, failedLines);

            // Check that we still have some lines to process
            if (rawLines.Length == 1)
                return (successfulReadings, failedLines);

            // We could copy the array of strings missing out the first element and use a
            // foreach here.  But to be honest we don't really need to do that, it would be
            // an inefficient use of memory and processing power.  So we go old school with
            // a 'for' statement.
            for (int index = 1; index < rawLines.Length; index++)
            {
                if (_meterCsvLineParser.TryParseMeterLine(rawLines[index], out MeterReading successfulReading))
                {
                    successfulReadings.Add(successfulReading);
                }
                else
                {
                    failedLines.Add(rawLines[index]);
                }
            }

            return (successfulReadings, failedLines);
        }

        // Public so I can easily test this method

    }
}
