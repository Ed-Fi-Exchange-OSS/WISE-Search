using System.Collections.Generic;
using WISEsearch.Web.Api.Domain.Index;

namespace WISEsearch.Web.Api.Domain.Interfaces
{
    public interface IIndexRequest
    {
        string IndexName { get; set; }
        string IdFieldName { get; set; }
        IEnumerable<IndexDocument> IndexRecords { get; set; } 
    }
}