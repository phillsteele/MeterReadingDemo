using MeterReader.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    public interface IRepo
    {
        Task AddFailedLine(string line);
        Task<MeterReading> AddReading(MeterReading meterReading);
        Task<Account> GetAccount(int accountId);
        Task<List<MeterReading>> GetDuplicateReadings();
        Task<List<string>> GetFailedLines();
        Task<List<MeterReading>> GetMeterReadings();
    }
}