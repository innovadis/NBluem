using System;
using System.Globalization;
using NBluem.Net.Request;

namespace NBluem.ValueObjects
{
    public class TtrsDateTime
    {
        private DateTime _inner;

        public TtrsDateTime(DateTime dateTime)
        {
            _inner = dateTime;
        }

        public static explicit operator DateTime(TtrsDateTime dateTime)
        {
            return dateTime._inner;
        }

        public override string ToString()
        {
            var info = new DateTimeFormatInfo();
            var culture = new CultureInfo("en-US");

            return _inner.ToString(info.RFC1123Pattern, culture);
        }

        public long Ticks => _inner.Ticks;

        public string Iso8601Timestamp => _inner.ToString("yyyyMMddHHmmssfff");

        public DateTime ToUtc()
        {
            return _inner.ToUtc();
        }
    }
}