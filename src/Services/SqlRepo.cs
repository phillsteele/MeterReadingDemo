using MeterReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReader.Services
{
    // This class is not implemented on purpose as I didn't want the overhead
    // of having the reader to worry about having a suitable db available.
    // Instead I just discuss what kind of functionality would go into this class.
    public class SqlRepo : IRepo
    {
        // It is envisaged that there would be an account table, with the primary key
        // equal to the accountId.  This would be prepopulated for the purpose of the demo.
        public async Task<Account> GetAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        // When adding readings we would make use of SQL's database integrtity, specifically
        // the ability to enforce foreign key constraints and the ability to enforce unique keys.
        // This would allow us to use the database to ensure that a specified account actually exists
        // and it would ensure that duplicate entries cannot be added by the correct construction
        // of a unique key.
        //
        // Using a database in this way prevents race conditions that exist with the simple in memory repo.
        public async Task<MeterReading> AddReading(MeterReading meterReading)
        {
            throw new NotImplementedException();
        }

        // Although not asked for having a record of failed lines could prove to be useful to help
        // develop data integrity for the future.
        public async Task AddFailedLine(string line)
        {
            throw new NotImplementedException();
        }

        // Although not asked for having a record of duplicate readings could prove to be useful to help
        // develop data integrity for the future.
        public async Task<List<MeterReading>> GetDuplicateReadings()
        {
            throw new NotImplementedException();
        }

        // Although not asked for having a record of failed lines could prove to be useful to help
        // develop data integrity for the future.
        public async Task<List<string>> GetFailedLines()
        {
            throw new NotImplementedException();
        }

        public Task<List<MeterReading>> GetMeterReadings()
        {
            throw new NotImplementedException();
        }
    }
}
