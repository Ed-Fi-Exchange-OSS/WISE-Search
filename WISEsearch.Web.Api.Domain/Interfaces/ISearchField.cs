namespace WISEsearch.Web.Api.Domain.Interfaces
{
    public interface ISearchField
    {
        string FieldName { get; set; }
        string Value { get; set; }
    }
}