using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class SearchResult
    {
        public int IndexDocumentId { get; set; }

        public string Explanation { get; set; }
        public List<SearchField> Fields { get; set; }
        public decimal Score { get; set; }
    }
}