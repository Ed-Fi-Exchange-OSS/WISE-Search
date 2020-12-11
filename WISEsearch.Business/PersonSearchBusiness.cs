using System;
using System.Collections.Generic;
using System.Configuration;
using EDFI.Modules.Search.Extensions;
using WISEsearch.Web.Api.Domain.Interfaces;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Business
{
    public interface IPersonSearchBusiness
    {
        long QueueBatchSearchRequest(IBatchPersonSearchRequest request);
        IEnumerable<SearchResult> Search(IPersonSearchRequest request);
    }

    public class PersonSearchBusiness : IPersonSearchBusiness
    {
        private readonly ISearchIndexBusiness _searchIndexBusiness;
        private string _personSearchIndex;

        public PersonSearchBusiness(ISearchIndexBusiness searchIndexBusiness)
        {
            _searchIndexBusiness = searchIndexBusiness;
            _personSearchIndex = ConfigurationManager.AppSettings["WISEsearch.PersonSearchIndex"];
        }

        public long QueueBatchSearchRequest(IBatchPersonSearchRequest request)
        {
            // TODO: save request
            throw new NotImplementedException("Batch requests have not been implemented yet");
        }

        public IEnumerable<SearchResult> Search(IPersonSearchRequest request)
        {
            var searchFields = ExtractSearchFields(request);
            var searchTokens = ExtractSearchTokens(request);

            return _searchIndexBusiness.Search(_personSearchIndex, searchFields, searchTokens, request.TopResultCount, explain: request.Explain);
        }

        private static IEnumerable<ISearchField> ExtractSearchFields(IPersonSearchRequest request)
        {
            yield return new SearchField {FieldName = "WiseId", Value = Convert.ToString(request.WiseId)};
            yield return new SearchField {FieldName = "LocalPersonId", Value = request.LocalPersonId};
            yield return new SearchField {FieldName = "FirstName", Value = request.FirstName};
            yield return new SearchField {FieldName = "MiddleName", Value = request.MiddleName};
            yield return new SearchField {FieldName = "LastName", Value = request.LastName};
            yield return new SearchField {FieldName = "BirthDate", Value = request.BirthDate.ToLuceneDateString()};
            yield return new SearchField {FieldName = "Gender", Value = request.Gender};
            yield return new SearchField {FieldName = "Suffix", Value = request.Suffix};
            yield return new SearchField { FieldName = "EducatorNumber", Value = Convert.ToString(request.EducatorNumber) };
        }

        private static IEnumerable<ISearchQueryParameterToken> ExtractSearchTokens(IPersonSearchRequest request)
        {
            yield return  new SearchQueryParameterToken {Name = "WiseIdExactMatchWeight", Value = Convert.ToString(request.WiseIdExactMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "WiseIdPartialMatchWeight", Value = Convert.ToString(request.WiseIdPartialMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "LocalRowKeyExactMatchWeight", Value = Convert.ToString(request.LocalRowKeyExactMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "MinimumFieldMatches", Value = Convert.ToString(request.MinimumFieldMatches)};
            yield return  new SearchQueryParameterToken {Name = "FirstNameWithSynonymsMatchWeight", Value = Convert.ToString(request.FirstNameWithSynonymsMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "FirstNameTolerance", Value = Convert.ToString(request.FirstNameTolerance)};
            yield return  new SearchQueryParameterToken {Name = "FirstNamePhoneticWeight", Value = Convert.ToString(request.FirstNamePhoneticWeight)};
            yield return  new SearchQueryParameterToken {Name = "MiddleNameWithSynonymsMatchWeight", Value = Convert.ToString(request.MiddleNameWithSynonymsMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "MiddleNameTolerance", Value = Convert.ToString(request.MiddleNameTolerance)};
            yield return  new SearchQueryParameterToken {Name = "MiddleNamePhoneticWeight", Value = Convert.ToString(request.MiddleNamePhoneticWeight)};
            yield return  new SearchQueryParameterToken {Name = "LastNameWithSynonymsMatchWeight", Value = Convert.ToString(request.LastNameWithSynonymsMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "LastNameTolerance", Value = Convert.ToString(request.LastNameTolerance)};
            yield return  new SearchQueryParameterToken {Name = "LastNamePhoneticWeight", Value = Convert.ToString(request.LastNamePhoneticWeight)};
            yield return  new SearchQueryParameterToken {Name = "BirthDateExactMatchWeight", Value = Convert.ToString(request.BirthDateExactMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "BirthDatePartialMatchWeight", Value = Convert.ToString(request.BirthDatePartialMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "GenderExactMatchWeight", Value = Convert.ToString(request.GenderExactMatchWeight)};
            yield return  new SearchQueryParameterToken {Name = "SuffixExactMatchWeight", Value = Convert.ToString(request.SuffixExactMatchWeight)};
            yield return new SearchQueryParameterToken { Name = "EducatorNumberExactMatchWeight", Value = Convert.ToString(request.SuffixExactMatchWeight) };
        }
    }
}
