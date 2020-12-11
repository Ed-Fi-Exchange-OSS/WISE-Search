using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class BatchPersonSearchResponse
    {
        public long BatchId { get; set; }
        public bool Success { get; set; }
        public IList<string> Messages { get; set; }
    }
}