using System;
using System.Collections.Generic;
using WISEsearch.Web.Api.Domain.Interfaces;

namespace WISEsearch.Web.Api.Domain.Search
{
    public class PersonSearchRequest : IPersonSearchRequest
    {
        public long? WiseId { get; set; }
        public string LocalPersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Suffix { get; set; }
        public long? EducatorNumber { get; set; }

        public IEnumerable<SearchQueryParameterToken> QueryParameterTokens { get; set; }
        public bool Explain { get; set; }
        public int TopResultCount { get; set; }
        public decimal WiseIdExactMatchWeight { get; set; }
        public decimal WiseIdPartialMatchWeight { get; set; }
        public decimal LocalRowKeyExactMatchWeight { get; set; }
        public int MinimumFieldMatches { get; set; }
        public decimal FirstNameWithSynonymsMatchWeight { get; set; }
        public decimal FirstNameTolerance { get; set; }
        public decimal FirstNamePhoneticWeight { get; set; }
        public decimal MiddleNameWithSynonymsMatchWeight { get; set; }
        public decimal MiddleNameTolerance { get; set; }
        public decimal MiddleNamePhoneticWeight { get; set; }
        public decimal LastNameWithSynonymsMatchWeight { get; set; }
        public decimal LastNameTolerance { get; set; }
        public decimal LastNamePhoneticWeight { get; set; }
        public decimal BirthDateExactMatchWeight { get; set; }
        public decimal BirthDatePartialMatchWeight { get; set; }
        public decimal GenderExactMatchWeight { get; set; }
        public decimal SuffixExactMatchWeight { get; set; }
        public decimal EducatorNumberExactMatchWeight { get; set; }
    }
}