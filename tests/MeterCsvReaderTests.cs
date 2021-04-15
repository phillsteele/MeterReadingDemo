using MeterReader.Model;
using MeterReader.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MeterReader.Tests
{
    [TestClass]
    public class MeterCsvReaderTests
    {
        [TestMethod]
        public void EmptyStringExpectEmptyLists()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("");

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void NullStringExpectEmptyLists()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv(null);

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void WhitespaceOnlyStringExpectEmptyLists()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("   \t   \r  \n  ");

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void HeadersAreInvalidExpectEmptyLists()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            mockValidator.Setup(v => v.ValidateHeaders(It.IsAny<string[]>(), It.IsAny<string[]>())).Returns(false);

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("abc");

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void OnlyHeadersExistExpectEmptyLists()
        {
            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            mockValidator.Setup(v => v.ValidateHeaders(It.IsAny<string[]>(), It.IsAny<string[]>())).Returns(true);

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("abc");

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void MeterLineIsValidExpectASuccessfulReading()
        {
            MeterReading expectedMeterReading = new MeterReading()
            {
                AccountId = 123,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00045"
            };

            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            mockValidator.Setup(v => v.ValidateHeaders(It.IsAny<string[]>(), It.IsAny<string[]>())).Returns(true);
            mockLineParser.Setup(p => p.TryParseMeterLine(It.IsAny<string>(), out expectedMeterReading)).Returns(true);

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("abc\r\n\r\n123");

            Assert.AreEqual(1, successFulReadings.Count);
            Assert.AreEqual(expectedMeterReading.AccountId, successFulReadings[0].AccountId);
            Assert.AreEqual(expectedMeterReading.MeterReadingDateTime, successFulReadings[0].MeterReadingDateTime);
            Assert.AreEqual(expectedMeterReading.MeterReadValue, successFulReadings[0].MeterReadValue);
            Assert.AreEqual(0, failedLines.Count);
        }

        [TestMethod]
        public void MeterLineIsInalidExpectASuccessfulReading()
        {
            MeterReading expectedMeterReading = new MeterReading()
            {
                AccountId = 123,
                MeterReadingDateTime = new DateTime(2005, 5, 21),
                MeterReadValue = "00045"
            };

            Mock<IMeterCsvValidator> mockValidator = new Mock<IMeterCsvValidator>();
            Mock<IMeterCsvLineParser> mockLineParser = new Mock<IMeterCsvLineParser>();

            mockValidator.Setup(v => v.ValidateHeaders(It.IsAny<string[]>(), It.IsAny<string[]>())).Returns(true);
            mockLineParser.Setup(p => p.TryParseMeterLine(It.IsAny<string>(), out expectedMeterReading)).Returns(false);

            MeterCsvReader meterCsvReader = new MeterCsvReader(mockValidator.Object, mockLineParser.Object);

            (var successFulReadings, var failedLines) = meterCsvReader.ParseCsv("abc\r\n\r\n123");

            Assert.AreEqual(0, successFulReadings.Count);
            Assert.AreEqual(1, failedLines.Count);
            Assert.AreEqual("123", failedLines[0]);
        }
    }
}
