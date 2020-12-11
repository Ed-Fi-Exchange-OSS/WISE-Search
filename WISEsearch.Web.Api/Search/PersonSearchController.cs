using System;
using System.Web.Http;
using WISEsearch.Business;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Web.Api.Search
{
    public interface IPersonSearchController
    {
        
    }

    public class PersonSearchController : ApiController, IPersonSearchController
    {
        private readonly IPersonSearchBusiness _personSearchBusiness;

        public PersonSearchController(IPersonSearchBusiness personSearchBusiness)
        {
            _personSearchBusiness = personSearchBusiness;
        }

        [HttpPost]
        public PersonSearchResponse Search(PersonSearchRequest request)
        {
            var response = new PersonSearchResponse();

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                response.Results = _personSearchBusiness.Search(request);
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Messages.Add(e.Message);
            }
            return response;

        }

        [HttpPost]
        public BatchPersonSearchResponse Search(BatchPersonSearchRequest request)
        {
            var response = new BatchPersonSearchResponse();

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                    response.BatchId = _personSearchBusiness.QueueBatchSearchRequest(request);
                    response.Success = true;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Messages.Add(e.Message);
            }
            return response;

        }
    }
}
