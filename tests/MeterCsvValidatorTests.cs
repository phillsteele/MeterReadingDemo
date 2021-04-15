using MeterReader.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MeterReader.Tests
{
    [TestClass]
    public class MeterCsvValidatorTests
    {
        [TestMethod]
        public void All3HeadersExistExpectSuccess()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            string[] validHeaders = new string[] { "A", "B", "C" };
            string[] headerTokens = new string[] { "C", "B", "A" };

            Assert.IsTrue(validator.ValidateHeaders(validHeaders, headerTokens));
        }

        [TestMethod]
        public void OneHeaderIsMissingExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            string[] validHeaders = new string[] { "A", "B", "C" };
            string[] headerTokens = new string[] { "A", "B" };

            Assert.IsFalse(validator.ValidateHeaders(validHeaders, headerTokens));
        }

        [TestMethod]
        public void ExtraHeaderExistsExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            string[] validHeaders = new string[] { "A", "B", "C" };
            string[] headerTokens = new string[] { "A", "B", "C", "D" };

            Assert.IsFalse(validator.ValidateHeaders(validHeaders, headerTokens));
        }

        [TestMethod]
        public void AccountIdIsPositiveIntegerExpectSuccess()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsTrue(validator.TryParseAccountId("78", out int accountId));
            Assert.AreEqual(78, accountId);
        }

        [TestMethod]
        public void AccountIdIsNegativeIntegerExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseAccountId("-78", out int accountId));
        }

        [TestMethod]
        public void AccountIdIsDecimalExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseAccountId("78.45", out int accountId));
        }

        [TestMethod]
        public void AccountIdIsNotANumberExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseAccountId("ABC", out int accountId));
        }

        [TestMethod]
        public void MeterReadingIsValidDateTimeExpectSuccess()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsTrue(validator.TryParseMeterReadingDateTime("20/05/2019 09:24", out DateTime meterReadingDateTime));
            Assert.AreEqual(new DateTime(2019, 05, 20, 9, 24, 0), meterReadingDateTime);
        }

        [TestMethod]
        public void MeterReadingIsInvalidDateTimeExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReadingDateTime("31/04/2019 09:24", out DateTime meterReadingDateTime));
        }

        [TestMethod]
        public void ReadingIs5DigitsExpectSuccess()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsTrue(validator.TryParseMeterReading("12345", out string meterReading));
            Assert.AreEqual("12345", meterReading);
        }

        [TestMethod]
        public void ReadingIs4DigitsExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReading("1234", out string meterReading));
        }

        [TestMethod]
        public void ReadingIs6DigitsExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReading("123456", out string meterReading));
        }

        [TestMethod]
        public void ReadingIsNegativeExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReading("-12345", out string meterReading));
        }

        [TestMethod]
        public void ReadingIsMissingExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReading("", out string meterReading));
        }

        [TestMethod]
        public void ReadingIsNotANumberExpectFailure()
        {
            MeterCsvValidator validator = new MeterCsvValidator();

            Assert.IsFalse(validator.TryParseMeterReading("12X45", out string meterReading));
        }
    }
}
