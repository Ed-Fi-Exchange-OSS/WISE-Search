using System.Collections.Generic;
using WISEsearch.Web.Api.Domain.Interfaces;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class BatchPersonSearchRequest : IBatchPersonSearchRequest
    {
        public IEnumerable<IPersonSearchRequest> PersonSearches { get; private set; }
    }
}