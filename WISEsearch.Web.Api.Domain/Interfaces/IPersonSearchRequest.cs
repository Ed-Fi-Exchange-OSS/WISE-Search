using System;
using System.Collections.Generic;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Web.Api.Domain.Interfaces
{
    public interface IPersonSearchRequest
    {
        long? WiseId { get;  }
        string LocalPersonId { get; }
        string FirstName { get; }
        string MiddleName { get; }
        string LastName { get; }
        DateTime? BirthDate { get; }
        string Gender { get; set; }
        string Suffix { get; set; }
        long? EducatorNumber { get; set; }
        
        IEnumerable<SearchQueryParameterToken> QueryParameterTokens { get; }
        bool Explain { get; set; }
        int TopResultCount { get; set; }
        decimal WiseIdExactMatchWeight { get; set; }
        decimal WiseIdPartialMatchWeight { get; set; }
        decimal LocalRowKeyExactMatchWeight { get; set; }
        int MinimumFieldMatches { get; set; }
        decimal FirstNameWithSynonymsMatchWeight { get; set; }
        decimal FirstNameTolerance { get; set; }
        decimal FirstNamePhoneticWeight { get; set; }
        decimal MiddleNameWithSynonymsMatchWeight { get; set; }
        decimal MiddleNameTolerance { get; set; }
        decimal MiddleNamePhoneticWeight { get; set; }
        decimal LastNameWithSynonymsMatchWeight { get; set; }
        decimal LastNameTolerance { get; set; }
        decimal LastNamePhoneticWeight { get; set; }
        decimal BirthDateExactMatchWeight { get; set; }
        decimal BirthDatePartialMatchWeight { get; set; }
        decimal GenderExactMatchWeight { get; set; }
        decimal SuffixExactMatchWeight { get; set; }
        decimal EducatorNumberExactMatchWeight { get; set; }
    }
}