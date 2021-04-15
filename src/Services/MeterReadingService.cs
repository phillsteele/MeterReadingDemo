using MeterReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterCsvReader _meterCsvReader;
        private readonly IRepo _repo;

        public MeterReadingService(
            IMeterCsvReader meterCsvReader,
            IRepo repo)
        {
            _meterCsvReader = meterCsvReader;
            _repo = repo;
        }

        public async Task<(int numberOfFailedReadings, int numberOfSuccessfulReadings)> ProcessReadings(string csvFile)
        {
            (var successFulReadings, var failedLines) = _meterCsvReader.ParseCsv(csvFile);

            // Check to see if there is anything to process once we parsed the file
            if (successFulReadings.Count == 0)
                return (successFulReadings.Count, failedLines.Count);

            int numberOfSuccessfulReadings = successFulReadings.Count;
            int numberOfFailedReadings = failedLines.Count;

            // Write the failed CSV lines off to the DB
            foreach (string failedLine in failedLines)
            {
                await _repo.AddFailedLine(failedLine);
            }

            foreach (var meterReading in successFulReadings)
            {
                var persistedMeterReading = await _repo.AddReading(meterReading);

                if (persistedMeterReading == null)
                {
                    numberOfSuccessfulReadings--;
                    numberOfFailedReadings++;
                }
            }

            return (numberOfSuccessfulReadings, numberOfFailedReadings);
        }

        public async Task<List<string>> GetFailedCsvLines()
        {
            return await _repo.GetFailedLines();
        }

        public async Task<List<MeterReading>> GetDuplicateMeterReadings()
        {
            return await _repo.GetDuplicateReadings();
        }

        public async Task<List<MeterReading>> GetMeterReadings()
        {
            return await _repo.GetMeterReadings();
        }
    }
}
