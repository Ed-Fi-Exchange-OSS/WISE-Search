using System;
using System.Linq;
using System.Web.Http;
using WISEsearch.Business;
using WISEsearch.Web.Api.Domain.Index;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Web.Api.Search
{
    public class SearchIndexController : ApiController
    {
        private readonly ISearchIndexBusiness _searchIndexBusiness;

        public SearchIndexController(ISearchIndexBusiness searchIndexBusiness)
        {
            _searchIndexBusiness = searchIndexBusiness;
        }

        [HttpPost]
        public BatchSearchResponse BatchSearch([FromBody] BatchSearchRequest request)
        {
            return new BatchSearchResponse
            {
                ReferenceId = request.ReferenceId,
                SearchResults = _searchIndexBusiness.Search(request),
                Success = true
            };
        }

        [HttpPost]
        public SearchResponse Search([FromBody] SearchRequest request)
        {

            return new SearchResponse
            {
                SearchResults = _searchIndexBusiness
                .Search(request)
                    .ToList(),
                Success = true
            };
        }

        [HttpPut]
        public IndexResponse Put([FromBody]IndexRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            _searchIndexBusiness.Index(request);
            return new IndexResponse {Success = true};
        }

        [HttpDelete]
        public DeleteIndexResponse Delete([FromBody]DeleteIndexRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            _searchIndexBusiness.DeleteIndexes(request);
            return new DeleteIndexResponse {Success = true};
        }

        [HttpGet]
        [HttpPost]
        public OptimizeIndexResponse Optimize([FromBody] OptimizeIndexRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            _searchIndexBusiness.Optimize(request);
            return new OptimizeIndexResponse {Success = true};
        }

        [HttpDelete]
        public ClearIndexResponse Clear([FromBody] ClearIndexesRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            _searchIndexBusiness.ClearAll(request);
            return new ClearIndexResponse {Success = true};

        }
    }
}
