using System.Collections.Generic;
using System.Web.Mvc;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.MVC.Models.Management
{
    public class SearchRequestViewModel
    {
        [AllowHtml]
        public string Query { get; set; }
        public string TargetIndex { get; set; }
        public IEnumerable<SearchField> SearchFields { get; set; }
        public IEnumerable<SearchQueryParameterToken> SearchParameters { get; set; }
        public int TopResults { get; set; }
        public bool Explain { get; set; }
    }
}