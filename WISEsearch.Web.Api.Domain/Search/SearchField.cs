using WISEsearch.Web.Api.Domain.Interfaces;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class SearchField : ISearchField
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}