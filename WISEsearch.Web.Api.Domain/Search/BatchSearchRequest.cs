using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class BatchSearchRequest
    {
        public string ReferenceId { get; set; }
        public int TopResults { get; set; }
        public bool Explain { get; set; }
        public string IndexName { get; set; }
        public string SearchQueryName { get; set; }
        public IEnumerable<SearchQueryParameterToken> SearchConfigurationParameters { get; set; }

        public List<Search> Searches { get; set; }
    }
}
