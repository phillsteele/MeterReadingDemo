using MeterReader.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    public class SimpleInMemoryRepo : IRepo
    {
        private Dictionary<int, Account> _accounts;

        // Our simple in memory repo will not start with any readings, and it will only persist to memory
        private Dictionary<MeterReadingUniqueKey, MeterReading> _meterReadings = new Dictionary<MeterReadingUniqueKey, MeterReading>();

        private List<string> _failedLines = new List<string>();
        private List<MeterReading> _duplicateReadings = new List<MeterReading>();

        public SimpleInMemoryRepo()
        {
            PopulateDemoData();
        }

        private void PopulateDemoData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MeterReader.Data.accounts.json";

            string rawJsonData;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                rawJsonData = reader.ReadToEnd();
            }

            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(rawJsonData);

            _accounts = new Dictionary<int, Account>();

            foreach (var account in accounts)
            {
                _accounts.Add(account.AccountId, account);
            }
        }

        // So, no real need here to use a Task, but the interface this will expose needs to be
        // used by repos that actually talk to real databases.  They will require access to async
        // methods for all db reads/writes.
        public Task<Account> GetAccount(int accountId)
        {
            if (_accounts.ContainsKey(accountId))
                return Task.FromResult(_accounts[accountId]);

            return Task.FromResult(default(Account));
        }

        // We will add a reading if one does not already exist.
        //
        // In this example we use a dictionary with a key based on the account Id and the date/time of the reading
        //
        // We are not attempting to force data integrity in this simple in memory repo.  A relational database
        // offers data constraints and unique indexes.  Both of which would be used in this scenario.
        // See the SqlRepo.cs for info on how this scenario would be handled in sql.
        public Task<MeterReading> AddReading(MeterReading meterReading)
        {
            if (!_accounts.ContainsKey(meterReading.AccountId))
                return Task.FromResult(default(MeterReading));

            var key = new MeterReadingUniqueKey(meterReading.AccountId, meterReading.MeterReadValue);

            // Check for duplicates
            // Though there is the question of what constitutes a duplicate.
            // The spec says: You should not be able to load the same entry twice
            // It doesn't say what constitutes 2 entries being the same.
            // It is feasible to have 2 meter readings that are the same separated by time.
            // For this scenario I am assuming that 2 meter readings are identical if they
            // are for the same accountId and have the same meter reading value.
            if (_meterReadings.TryAdd(key, meterReading) == false)
            {
                _duplicateReadings.Add(meterReading);
                return Task.FromResult(default(MeterReading));
            }

            return Task.FromResult(_meterReadings[key]);
        }

        public Task<List<MeterReading>> GetDuplicateReadings()
        {
            return Task.FromResult(_duplicateReadings);
        }

        public Task<List<MeterReading>> GetMeterReadings()
        {
            return Task.FromResult(_meterReadings.Values.ToList());
        }

        public Task<List<string>> GetFailedLines()
        {
            return Task.FromResult(_failedLines);
        }

        public Task AddFailedLine(string line)
        {
            _failedLines.Add(line);

            return Task.CompletedTask;
        }
    }
}
