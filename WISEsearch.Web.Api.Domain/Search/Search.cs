using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class Search
    {
        public string ReferenceId { get; set; }
        public IEnumerable<SearchField> SearchFields { get; set; }
    }
}