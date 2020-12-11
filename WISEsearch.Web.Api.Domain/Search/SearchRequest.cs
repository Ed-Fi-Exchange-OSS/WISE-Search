using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class SearchRequest : BaseRequest
    {
        public int TopResults { get; set; }

        public bool Explain { get; set; }

        public string IndexName { get; set; }
        public string SearchQueryName { get; set; }
        public IEnumerable<SearchField> SearchFields { get; set; }
        public IEnumerable<SearchQueryParameterToken> SearchConfigurationParameters { get; set; }
    }
}