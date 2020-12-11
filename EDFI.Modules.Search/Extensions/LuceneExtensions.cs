using System;
using Lucene.Net.Documents;

namespace EDFI.Modules.Search.Extensions
{
    public static class LuceneExtensions
    {
        public static string ToLuceneDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.ToLuceneDateString()
                : null;
        }

        public static string ToLuceneDateString(this DateTime dateTime)
        {
            return DateTools.DateToString(dateTime, DateTools.Resolution.DAY);
        }
    }
}
