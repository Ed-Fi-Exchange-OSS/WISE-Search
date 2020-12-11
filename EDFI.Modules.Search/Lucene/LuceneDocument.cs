using System.Collections.Generic;
using System.Linq;

namespace EDFI.Modules.Search.Lucene
{
    public class LuceneDocument
    {
        public LuceneDocument()
        {
            Fields = Enumerable.Empty<LuceneField>();
        }

        public LuceneDocument(IEnumerable<LuceneField> fields)
        {
            Fields = fields;
        }


        public IEnumerable<LuceneField> Fields { get; set; }
    }
}