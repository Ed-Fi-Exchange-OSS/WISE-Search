using System.Collections.Generic;

namespace EDFI.Modules.Search.Lucene
{
    public class LuceneSearchResult
    {
        public int IndexDocumentId { get; set; }

        public string Explanation { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public decimal Score { get; set; }
    }
}