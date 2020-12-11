using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EDFI.Modules.Search.Extensions;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace EDFI.Modules.Search.Lucene
{
    public interface ILuceneQueryParser
    {
        Query ParseQuery(string searchName, Dictionary<string, string> fields);
    }

    public class LuceneQueryParser : ILuceneQueryParser
    {
        private readonly IPhoneticEncoder _phoneticEncoder;

        public LuceneQueryParser(IPhoneticEncoder phoneticEncoder)
        {
            _phoneticEncoder = phoneticEncoder;
        }

        public Query ParseQuery(string queryXml, Dictionary<string, string> fields)
        {
            var rootQueryElement = XDocument.Parse(queryXml)
                .Elements()
                .Single();

            return ParseQuery(rootQueryElement, fields);
            //var rootQuery = new BooleanQuery(true);
            //foreach (var element in query.Elements())
            //{
            //    var luceneQuery = ParseQuery(element, fields);
            //    if (luceneQuery != null)
            //    {
            //        rootQuery.Add(luceneQuery, Occur.MUST);
            //    }
            //}
            //return rootQuery;
        }

        private Query ParseQuery(XElement xmlQuery, Dictionary<string, string> serachFields)
        {
            switch (xmlQuery.Name.LocalName)
            {
                case "alwaysTrueQuery": return new MatchAllDocsQuery();
                case "booleanQuery": return ParseBooleanQuery(xmlQuery, serachFields);
                case "fieldQuery": return ParseFieldQuery(xmlQuery, serachFields);
                default: throw new ArgumentException("Query Parsing Error: Unrecognized search query element : '" + xmlQuery.Name.LocalName + "'");
            }
        }

        private Query ParseFieldQuery(XElement fieldQuery, Dictionary<string, string> serachFields)
        {
            var indexField = fieldQuery.AttributeValueOrNull("indexField").ErrorIfNull("Query Parsing Error: 'indexField' attribute was not found on for this field query: " + fieldQuery);
            var searchField = fieldQuery.AttributeValueOrNull("searchField").ErrorIfNull("Query Parsing Error: 'searchField' attribute was not found on for this field query: " + fieldQuery);
            var matchType = fieldQuery.AttributeValueOrNull("matchType") ?? "exact";

            var searchTerms = GetSearchTerms(serachFields, searchField);
            if (searchTerms.Any() == false) return null;

            if (matchType == "phonetic")
            {
                searchTerms = searchTerms
                    .SelectMany(term => _phoneticEncoder.GetPhoneticKeys(term).Select(key => key));
            }

            var booleanQuery = new BooleanQuery();
            var tolerance = fieldQuery.AttributeValueOrNull("tolerance").ParseFloatOrNull();
            var fuzzyPrefixLength = fieldQuery.AttributeValueOrNull("prefixLength").ParseIntOrNull() ?? 0;
            foreach (var term in searchTerms)
            {
                switch (matchType)
                {
                    case "phonetic" :
                    case "exact": booleanQuery.Add(new TermQuery(new Term(indexField, term.ToLower())), Occur.SHOULD);
                        break;
                    case "fuzzy": booleanQuery.Add(new FuzzyQuery(new Term(indexField, term.ToLower()), tolerance.ErrorIfNull("'tolerance' attribute is required for query '{0}'", fieldQuery), fuzzyPrefixLength), Occur.SHOULD);
                        break;
                }
                
            }

            var weight = fieldQuery.AttributeValueOrNull("weight").ParseFloatOrNull();
            if (weight.HasValue)
            {
                booleanQuery.Boost = weight.Value;
            }
            return booleanQuery;
        }


        private Query ParseBooleanQuery(XElement booleanQuery, Dictionary<string, string> serachFields)
        {
            var searchOperator = booleanQuery.AttributeValueOrNull("operator").ErrorIfNull("Query Parsing Error: Boolean query is missing operator attribute: " + booleanQuery);
            int? minimumFieldMatches = booleanQuery.AttributeValueOrNull("minimumFieldMatches").ParseIntOrNull();
            var disableCoord = booleanQuery.AttributeValueOrNull("disableCoord").ParseOrDefault(false);

            Occur occurance;
            switch (searchOperator)
            {
                case "and" : occurance = Occur.MUST;
                    break;
                case "or" : occurance = Occur.SHOULD;
                    break;
                case "not" : occurance =  Occur.MUST_NOT;
                    break;
                default:
                    throw new Exception("Query Parsing Error: Unknown booleanQuery operator: " + searchOperator + ".  Use \"and\", \"or\", or \"not\"");
            }

            var query = new BooleanQuery(disableCoord);
            foreach (var childQueryElement in booleanQuery.Elements())
            {
                var luceneQuery = ParseQuery(childQueryElement, serachFields);
                if (luceneQuery != null)
                {
                    query.Add(luceneQuery, occurance);
                }
            }

            if (minimumFieldMatches.HasValue)
            {
                query.MinimumNumberShouldMatch = minimumFieldMatches.Value;
            }
            return query;
        }

        private static IEnumerable<string> GetSearchTerms(Dictionary<string, string> serachFields, string fieldName)
        {
            if (serachFields.ContainsKey(fieldName) == false) return Enumerable.Empty<string>();
            var searchText = serachFields[fieldName];
            if (string.IsNullOrWhiteSpace(searchText))
                return Enumerable.Empty<string>();

            return searchText
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .Union(new [] {searchText.Replace(" ", string.Empty)})
                .Distinct();
        }
    }
}
