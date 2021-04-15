using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReader.Model
{
    public class MeterReadingUniqueKey
    {
        // Make this class immutable
        public MeterReadingUniqueKey(int accountId, string meterReadingValue)
        {
            AccountId = accountId;
            MeterReadingValue = meterReadingValue;
        }

        public int AccountId { get; }
        public string MeterReadingValue { get; }

        public override bool Equals(object obj)
        {
            MeterReadingUniqueKey objToCompare = obj as MeterReadingUniqueKey;

            if (objToCompare == null)
                return false;

            return AccountId == objToCompare.AccountId && MeterReadingValue == objToCompare.MeterReadingValue;
        }

        public override int GetHashCode()
        {
            return AccountId.GetHashCode() ^ MeterReadingValue.GetHashCode();
        }
    }
}
