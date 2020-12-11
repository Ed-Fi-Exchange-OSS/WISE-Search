using System.Collections.Generic;
using System.Linq;
using WISEsearch.Web.Api.Domain.Interfaces;

namespace WISEsearch.Web.Api.Domain.Index
{
    public class IndexRequest : BaseRequest, IIndexRequest
    {
        public IndexRequest()
        {
            IndexRecords = Enumerable.Empty<IndexDocument>();
        }

        public IndexRequest(string indexName, string idFieldName, IEnumerable<IndexDocument> indexRecords)
        {
            IndexName = indexName;
            IdFieldName = idFieldName;
            IndexRecords = indexRecords;
        }

        public string IndexName { get; set; }
        public string IdFieldName { get; set; }
        public IEnumerable<IndexDocument> IndexRecords { get; set; }
    }
}