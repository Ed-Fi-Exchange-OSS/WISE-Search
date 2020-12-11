using System.Collections.Generic;
using EDFI.Modules.Search.Extensions;
using EDFI.Modules.Search.Lucene;

namespace EDFI.Modules.Search
{
    public interface IIndexService
    {

        IEnumerable<LuceneSearchResult> Search(string queryXml, string targetIndex, Dictionary<string, string> fields, Dictionary<string, string> searchTokens, int topResults, bool explain = false);
        void CreateOrUpdateIndexes(string indexName, string idFieldName, IEnumerable<LuceneDocument> indexDocuments);
        void DeleteIndexes(string indexName, string idFieldName, IEnumerable<string> fieldValueIds);
        void ClearIndexes(string indexName);
        void Optimize(string indexName);
    }

    public class IndexService : IIndexService
    {
        private readonly ILuceneQueryParser _luceneQueryParser;
        private readonly ILuceneSearcherContextFactory _luceneSearcherContextFactory;

        public IndexService(ILuceneQueryParser luceneQueryParser, ILuceneSearcherContextFactory luceneSearcherContextFactory)
        {
            _luceneQueryParser = luceneQueryParser;
            _luceneSearcherContextFactory = luceneSearcherContextFactory;
        }

        public IEnumerable<LuceneSearchResult> Search(string queryXml, string targetIndex, Dictionary<string, string> fields, Dictionary<string, string> searchTokens, int topResults, bool explain = false)
        {
            queryXml.ErrorIfNull("queryXml is required");
            targetIndex.ErrorIfNull("targetIndex is required");

            queryXml = queryXml
                .ReplaceTokens(searchTokens);

            var query = _luceneQueryParser.ParseQuery(queryXml, fields);

            using (var searcherContext = _luceneSearcherContextFactory.CreateSearchContext(targetIndex))
            {
                return searcherContext.Search(query, topResults, explain);
            }
        }

        public void CreateOrUpdateIndexes(string indexName, string idFieldName, IEnumerable<LuceneDocument> indexDocuments)
        {
            using (var searcherContext = _luceneSearcherContextFactory.CreateSearchContext(indexName))
            {
                searcherContext.CreateOrUpdateIndex(idFieldName, indexDocuments);
            }
        }

        public void DeleteIndexes(string indexName, string idFieldName, IEnumerable<string> fieldValueIds)
        {
            using (var searcherContext = _luceneSearcherContextFactory.CreateSearchContext(indexName))
            {
                searcherContext.DeleteIndexes(idFieldName, fieldValueIds);
            }
        }

        public void ClearIndexes(string indexName)
        {
            using (var searcherContext = _luceneSearcherContextFactory.CreateSearchContext(indexName))
            {
                searcherContext.ClearIndexes();
            }
        }

        public void Optimize(string indexName)
        {
            using (var searcherContext = _luceneSearcherContextFactory.CreateSearchContext(indexName))
            {
                searcherContext.Optimize();
            }
        }
    }
}