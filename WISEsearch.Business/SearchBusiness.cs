using System;
using System.Collections.Generic;
using System.Linq;
using EDFI.Modules.Search;
using EDFI.Modules.Search.Extensions;
using EDFI.Modules.Search.Lucene;
using WISEsearch.Web.Api.Domain.Index;
using WISEsearch.Web.Api.Domain.Interfaces;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Business
{

    public interface ISearchIndexBusiness
    {
        void Index(IIndexRequest indexRequest);
        void DeleteIndexes(DeleteIndexRequest request);
        void ClearAll(ClearIndexesRequest request);
        void Optimize(OptimizeIndexRequest request);
        IEnumerable<SearchResult> Search(string searchQueryName, IEnumerable<ISearchField> fields, IEnumerable<ISearchQueryParameterToken> searchTokens, int topResults, bool explain = false, string indexName = null);
        IEnumerable<BatchSearchResult> Search(BatchSearchRequest batchSearchRequest);
        IEnumerable<SearchResult> Search(SearchRequest searchRequest);
    }

    public class SearchIndexBusiness : ISearchIndexBusiness
    {
        private readonly IQueryBusiness _queryBusiness;
        private readonly IIndexService _indexService;

        public SearchIndexBusiness(IQueryBusiness queryBusiness, IIndexService indexService)
        {
            _queryBusiness = queryBusiness;
            _indexService = indexService;
        }

        public IEnumerable<BatchSearchResult> Search(BatchSearchRequest batchSearchRequest)
        {
            foreach (var search in batchSearchRequest.Searches)
            {
                var results = Search(
                    batchSearchRequest.SearchQueryName,
                    search.SearchFields,
                    batchSearchRequest.SearchConfigurationParameters,
                    batchSearchRequest.TopResults,
                    batchSearchRequest.Explain,
                    batchSearchRequest.IndexName);

                yield return new BatchSearchResult
                {
                    ReferenceId = search.ReferenceId,
                    SearchResults = results.ToList()
                };

            }
        }

        
        public IEnumerable<SearchResult> Search(
            SearchRequest searchRequest)
        {
            return Search(
                    searchRequest.SearchQueryName,
                    searchRequest.SearchFields,
                    searchRequest.SearchConfigurationParameters,
                    searchRequest.TopResults,
                    searchRequest.Explain,
                    searchRequest.IndexName);
        }

        public IEnumerable<SearchResult> Search(
            
            string searchQueryName,
            IEnumerable<ISearchField> fields,
            IEnumerable<ISearchQueryParameterToken> searchParameters,
            int topResults,
            bool explain = false,
            string indexName = null)
        {
            var queryConfiguration = _queryBusiness
                .GetQueryConfiguration(searchQueryName)
                .ErrorIfNull("Unable to find configuration for {0}", searchQueryName);


            fields.ErrorIfNull("Search fields cannot be null");
            searchParameters = searchParameters ?? Enumerable.Empty<ISearchQueryParameterToken>();
            var fieldLookup = fields.ToDictionary(x => x.FieldName, x => x.Value);
            var searchParameterLookup = searchParameters.ToDictionary(x => x.Name, x => x.Value);
            var targetIndex = indexName ?? queryConfiguration.TargetIndex;
            return _indexService.Search(queryConfiguration.Query, targetIndex, fieldLookup,
                searchParameterLookup, topResults, explain)
                .Select(result => new SearchResult
                {
                    IndexDocumentId = result.IndexDocumentId,
                    Fields = result.Fields
                        .Select(field => new SearchField
                        {
                            FieldName = field.Key,
                            Value = field.Value
                        })
                        .ToList(),
                    Score = result.Score,
                    Explanation = result.Explanation
                });
        }

        public void Index(IIndexRequest indexRequest)
        {
            var luceneDocuments = indexRequest.IndexRecords
                .Select(record => new LuceneDocument
                {
                    Fields = record.Fields
                        .Select(field => new LuceneField(field.Name, field.Value, field.IsAnalyzed, field.IsStored, field.UseTermVector))
                });

            _indexService.CreateOrUpdateIndexes(indexRequest.IndexName, indexRequest.IdFieldName, luceneDocuments);
        }

        public void DeleteIndexes(DeleteIndexRequest request)
        {
            _indexService.DeleteIndexes(request.IndexName, request.IdFieldName, request.DeleteIds);
        }

        public void Optimize(OptimizeIndexRequest request)
        {
            _indexService.Optimize(request.IndexName);
        }

        public void ClearAll(ClearIndexesRequest request)
        {
            _indexService.ClearIndexes(request.IndexName);
        }

    }
}