using MeterReader.Model;
using MeterReader.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace MeterReader.Tests
{
    [TestClass]
    public class SimpleInMemoryRepoTests
    {
        [TestMethod]
        public async Task GetAccountThatExistsExpectAccountReturned()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            Account account = await inMemRepo.GetAccount(1234);

            Assert.IsNotNull(account);

            Assert.AreEqual(1234, account.AccountId);
            Assert.AreEqual("Freya", account.FirstName);
            Assert.AreEqual("Test", account.LastName);
        }

        [TestMethod]
        public async Task GetAccountThatDoesNotExistExpectNullReturned()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            Account account = await inMemRepo.GetAccount(999999);

            Assert.IsNull(account);
        }

        [TestMethod]
        public async Task AddSingleReadingAccountDoesNotExistExpectNull()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            MeterReading meterReading = new MeterReading()
            {
                AccountId = 123,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00063"
            };

            MeterReading reading = await inMemRepo.AddReading(meterReading);

            Assert.IsNull(reading);
        }

        [TestMethod]
        public async Task AddSingleReadingExpectReadingReturned()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            MeterReading meterReading = new MeterReading()
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00063"
            };

            MeterReading reading = await inMemRepo.AddReading(meterReading);

            Assert.IsNotNull(reading);

            Assert.AreEqual(1234, reading.AccountId);
            Assert.AreEqual(new DateTime(2005, 5, 21), reading.MeterReadingDateTime);
            Assert.AreEqual("00063", reading.MeterReadValue);
        }

        [TestMethod]
        public async Task AddTwoDifferentReadingsExpectBothToBeAdded()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            MeterReading meterReading1 = new MeterReading()
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00063"
            };

            MeterReading meterReading2 = new MeterReading()
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00064"
            };

            MeterReading readingTask1 = await inMemRepo.AddReading(meterReading1);
            MeterReading readingTask2 = await inMemRepo.AddReading(meterReading2);

            Assert.IsNotNull(readingTask1);
            Assert.IsNotNull(readingTask2);
        }

        [TestMethod]
        public async Task AddTwoReadingsWithTheSameKeyExpectNullFromSecondEntry()
        {
            SimpleInMemoryRepo inMemRepo = new SimpleInMemoryRepo();

            MeterReading meterReading1 = new MeterReading()
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00063"
            };

            MeterReading readingTask1 = await inMemRepo.AddReading(meterReading1);
            MeterReading readingTask2 = await inMemRepo.AddReading(meterReading1);

            Assert.IsNotNull(readingTask1);
            Assert.IsNull(readingTask2);
        }
    }
}
