// Imported from: https://github.com/NielsKuhnel/NrtManager/tree/master/Lucene.Net.Contrib.Management

using Lucene.Net.Search;

namespace EDFI.Modules.Search.Lucene
{
    public interface ISearcherWarmer
    {
        void Warm(IndexSearcher s);
    }
}