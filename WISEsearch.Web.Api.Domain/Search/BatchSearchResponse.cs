using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class BatchSearchResponse : BaseResponse
    {
        public string ReferenceId { get; set; }
        public IEnumerable<BatchSearchResult> SearchResults { get; set; }
    }
}