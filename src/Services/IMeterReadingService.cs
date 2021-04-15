using MeterReader.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    public interface IMeterReadingService
    {
        Task<List<MeterReading>> GetDuplicateMeterReadings();
        Task<List<string>> GetFailedCsvLines();
        Task<List<MeterReading>> GetMeterReadings();
        Task<(int numberOfFailedReadings, int numberOfSuccessfulReadings)> ProcessReadings(string csvFile);
    }
}