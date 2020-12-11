using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class SearchResponse : BaseResponse
    {
        public List<SearchResult> SearchResults { get; set; }
    }
}