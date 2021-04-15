using System;

namespace MeterReader.Services
{
    public interface IMeterCsvValidator
    {
        bool TryParseAccountId(string token, out int accountId);
        bool TryParseMeterReading(string token, out string meterReading);
        bool TryParseMeterReadingDateTime(string token, out DateTime meterReadingDateTime);
        bool ValidateHeaders(string[] validHeaders, string[] headerTokens);
    }
}