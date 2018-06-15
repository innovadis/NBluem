using System;
using HashidsNet;

namespace NBluem.ValueObjects
{
    public class EntranceCode
    {
        private readonly string _inner;

        public EntranceCode()
        {
            _inner = Generate();
        }

        public EntranceCode(TtrsDateTime dateTime)
        {
            _inner = Generate(dateTime);
        }

        public EntranceCode(string entranceCode)
        {
            _inner = entranceCode;
        }

        public override string ToString()
        {
            return _inner;
        }

        public static string Generate()
        {
            return Generate(new TtrsDateTime(DateTime.Now));
        }

        public static string Generate(TtrsDateTime dateTime)
        {
            var hashids = new Hashids(Configuration.BluemConfiguration.Config.HashSalt, Configuration.BluemConfiguration.Config.HashLength);
            return hashids.EncodeLong(dateTime.Ticks) + dateTime.Iso8601Timestamp;
        }
    }
}
