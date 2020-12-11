using DPI.Web.Api.Client;
using WISEsearch.Web.Api.Domain.Index;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Web.Api.Client.Search
{
    public interface ISearchIndexService
    {
        SearchResponse Search(SearchRequest request);
        IndexResponse Put(IndexRequest request);
        OptimizeIndexResponse Optimize(OptimizeIndexRequest request);
        DeleteIndexResponse Delete(DeleteIndexRequest request);
        ClearIndexResponse Clear(ClearIndexesRequest request);
        BatchSearchResponse Search(BatchSearchRequest request);
    }

    public class SearchIndexService : ISearchIndexService
    {
        private readonly IWebApiService _webApiService;
        private readonly string _wiseSearchUrl;

        public SearchIndexService() : this(new WebApiService())
        {
        }

        public SearchIndexService(IWebApiService webApiService)
        {
            _webApiService = webApiService;
            _wiseSearchUrl = AppConfiguration.GetWiseSearchBaseUrl();
        }

        public BatchSearchResponse Search(BatchSearchRequest request)
        {
            return _webApiService.Post<BatchSearchResponse>(_wiseSearchUrl, "searchindex/batchsearch", request);
        }

        public SearchResponse Search(SearchRequest request)
        {
            return _webApiService.Post<SearchResponse>(_wiseSearchUrl, "searchindex/search", request);
        }

        public IndexResponse Put(IndexRequest request)
        {
            return _webApiService.Put<IndexResponse>(_wiseSearchUrl, "searchindex/put", request);
        }

        public OptimizeIndexResponse Optimize(OptimizeIndexRequest request)
        {
            return _webApiService.Post<OptimizeIndexResponse>(_wiseSearchUrl, "searchindex/optimize", request);
        }

        public DeleteIndexResponse Delete(DeleteIndexRequest request)
        {
            return _webApiService.Delete<DeleteIndexResponse>(_wiseSearchUrl, "searchindex/delete", request);
        }

        public ClearIndexResponse Clear(ClearIndexesRequest request)
        {
            return _webApiService.Delete<ClearIndexResponse>(_wiseSearchUrl, "searchindex/clear", request);
        }
    }
}