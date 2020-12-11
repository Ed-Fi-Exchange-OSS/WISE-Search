using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain.Index
{
    public class IndexDocument
    {

        public IndexDocument()
        {
        }
        public IndexDocument(IEnumerable<IndexField> fields)
        {
            Fields = fields;
        }

        public IEnumerable<IndexField> Fields { get; set; }
    }
}