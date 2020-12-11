using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using EDFI.Modules.Search;
using EDFI.Modules.Search.Lucene;
using WISEsearch.Business;
using WISEsearch.MVC.Models.Management;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.MVC.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IQueryBusiness _queryBusiness;
        private readonly IIndexService _indexService;
        private readonly ILuceneSearcherContextFactory _luceneSearcherContextFactory;

        public ManagementController(IQueryBusiness queryBusiness, IIndexService indexService, ILuceneSearcherContextFactory luceneSearcherContextFactory)
        {
            _queryBusiness = queryBusiness;
            _indexService = indexService;
            _luceneSearcherContextFactory = luceneSearcherContextFactory;
        }

        public ActionResult Explorer()
        {
            var configurations = _queryBusiness.GetQueryConfigurations();
            var model = new QueryVisualizerViewModel(configurations);
            return View(model);
        }

        public JsonResult Search(SearchRequestViewModel model)
        {
            var searchFields = (model.SearchFields ?? Enumerable.Empty<SearchField>()).ToDictionary(x => x.FieldName, x => x.Value);
            var searchParameters = (model.SearchParameters ?? Enumerable.Empty<SearchQueryParameterToken>()).ToDictionary(x => x.Name, x => x.Value);

            var timer = Stopwatch.StartNew();
            var results = _indexService.Search(model.Query, model.TargetIndex, searchFields, searchParameters, model.TopResults, model.Explain)
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
            timer.Stop();
            
            return Json(new
            {
                Results = results,
                Time = timer.Elapsed.TotalSeconds
            });
        }

        public JsonResult IndexDetails(string id)
        {
            using (var searchContext = _luceneSearcherContextFactory.CreateSearchContext(id))
            {
                var timer = Stopwatch.StartNew();

                var results = searchContext.GetFirstResults(5)
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
                timer.Stop();

                return Json(new
                {
                    DocumentCount = searchContext.GetDocumentCount(),
                    Results = results,
                    Time = timer.Elapsed.TotalSeconds
                    
                });
            }
        }
    }
}