using WISEsearch.Web.Api.Domain.Interfaces;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class SearchQueryParameterToken : ISearchQueryParameterToken
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}