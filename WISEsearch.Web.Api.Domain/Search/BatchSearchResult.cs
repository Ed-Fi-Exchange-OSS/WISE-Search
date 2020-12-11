using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class BatchSearchResult
    {
        public string ReferenceId { get; set; }
        public List<SearchResult> SearchResults { get; set; }
    }
}