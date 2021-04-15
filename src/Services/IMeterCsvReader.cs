using MeterReader.Model;
using System.Collections.Generic;

namespace MeterReader.Services
{
    public interface IMeterCsvReader
    {
        (List<MeterReading>, List<string>) ParseCsv(string rawCsv);
    }
}