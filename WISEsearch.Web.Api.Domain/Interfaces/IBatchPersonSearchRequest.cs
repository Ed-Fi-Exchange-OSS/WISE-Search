using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Interfaces
{
    public interface IBatchPersonSearchRequest
    {
        IEnumerable<IPersonSearchRequest> PersonSearches { get; }
    }
}