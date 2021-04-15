using MeterReader.Model;

namespace MeterReader.Services
{
    public interface IMeterCsvLineParser
    {
        bool TryParseMeterLine(string csvMeterLine, out MeterReading successfulReading);
    }
}