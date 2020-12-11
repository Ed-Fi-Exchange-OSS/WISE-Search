using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Index
{
    public class DeleteIndexRequest : BaseRequest
    {
        public string IndexName { get; set; }
        public string IdFieldName { get; set; }
        public IEnumerable<string> DeleteIds { get; set; }
    }
}
