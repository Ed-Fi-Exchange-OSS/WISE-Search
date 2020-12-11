using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class PersonSearchResponse: BaseResponse
    {
        public IEnumerable<SearchResult> Results { get; set; }
    }
}