namespace WISEsearch.Web.Api.Domain.Index
{
    public class ClearIndexesRequest : BaseRequest
    {
        public string IndexName { get; set; }
    }
}