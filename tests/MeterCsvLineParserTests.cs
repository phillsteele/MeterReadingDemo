using MeterReader.Model;
using MeterReader.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MeterReader.Tests
{
    [TestClass]
    public class MeterCsvLineParserTests
    {
        [TestMethod]
        public void TokenLengthIs2ExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "abc,xyz";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void TokenLengthIs4ExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "abc1,abc2,abc3,abc4";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void TokensAreEmptyExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void AccountIdTokenIsInvalidExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            int expectedAccountId = 123;
            mockValidator.Setup(v => v.TryParseAccountId(It.IsAny<string>(), out expectedAccountId)).Returns(false);

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "A,B,C";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void MeterReadingDateTimeIsInvalidExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            int expectedAccountId = 123;
            DateTime expectedMeterReadingDateTime = new DateTime(2000, 05, 21);

            mockValidator.Setup(v => v.TryParseAccountId(It.IsAny<string>(), out expectedAccountId)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReadingDateTime(It.IsAny<string>(), out expectedMeterReadingDateTime)).Returns(false);

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "A,B,C";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void MeterReadingIsInvalidExpectFailure()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            int expectedAccountId = 123;
            DateTime expectedMeterReadingDateTime = new DateTime(2000, 05, 21);
            string expectedMeterReading = "00012";

            mockValidator.Setup(v => v.TryParseAccountId(It.IsAny<string>(), out expectedAccountId)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReadingDateTime(It.IsAny<string>(), out expectedMeterReadingDateTime)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReading(It.IsAny<string>(), out expectedMeterReading)).Returns(false);

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "A,B,C";

            Assert.IsFalse(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void TokensAreValidExpectSuccess()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            int expectedAccountId = 123;
            DateTime expectedMeterReadingDateTime = new DateTime(2000, 05, 21);
            string expectedMeterReading = "00012";

            mockValidator.Setup(v => v.TryParseAccountId(It.IsAny<string>(), out expectedAccountId)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReadingDateTime(It.IsAny<string>(), out expectedMeterReadingDateTime)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReading(It.IsAny<string>(), out expectedMeterReading)).Returns(true);

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "A,B,C";

            Assert.IsTrue(csvReader.TryParseMeterLine(tokens, out MeterReading meterReading));
        }

        [TestMethod]
        public void TokensAreValidExpectMeterReadingRecordReturned()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();

            int expectedAccountId = 123;
            DateTime expectedMeterReadingDateTime = new DateTime(2000, 05, 21);
            string expectedMeterReading = "00012";

            mockValidator.Setup(v => v.TryParseAccountId(It.IsAny<string>(), out expectedAccountId)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReadingDateTime(It.IsAny<string>(), out expectedMeterReadingDateTime)).Returns(true);
            mockValidator.Setup(v => v.TryParseMeterReading(It.IsAny<string>(), out expectedMeterReading)).Returns(true);

            MeterCsvLineParser csvReader = new MeterCsvLineParser(mockValidator.Object);

            string tokens = "A,B,C";

            csvReader.TryParseMeterLine(tokens, out MeterReading meterReading);

            Assert.AreEqual(expectedAccountId, meterReading.AccountId);
            Assert.AreEqual(expectedMeterReadingDateTime, meterReading.MeterReadingDateTime);
            Assert.AreEqual(expectedMeterReading, meterReading.MeterReadValue);
        }
    }
}
