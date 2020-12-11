using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using WISEsearch.Web.Api.Client.Search;
using WISEsearch.Web.Api.Domain.Index;
using WISEsearch.Web.Api.Domain.Search;

namespace WISEsearch.Web.Api.Client.Integration.Tests.Search
{
    [TestFixture(Category = "Integration")]
    public class SearchServiceTests
    {
        private ISearchIndexService CreateIndexService()
        {
            return new SearchIndexService();
        }

        [SetUp]
        public void CreateOrUpdateIndex()
        {
            var searchService = CreateIndexService();
            var request = CreateIndexRequest();
            searchService.Put(request);
        }

        [Test]
        public void Search_ReturnSingleMatch()
        {
            // Arrange
            var searchService = CreateIndexService();
            var request = new SearchRequest
            {
                TopResults = 10,
                SearchQueryName = "TestSearch",
                SearchFields = new []
                {
                    new SearchField{ FieldName = "FirstName", Value = "Fred"},
                    new SearchField{ FieldName = "LastName", Value = "Flintstone"},
                }
            };

            // Act
            var actual = searchService.Search(request);

            // Assert
            actual.Should().NotBeNull();
            //actual.Messages.FirstOrDefault().Should().BeEmpty();
            actual.Success.Should().BeTrue();
            actual.SearchResults.Should().HaveCount(1);
        }

        [Ignore]
        [Test]
        public void Search_ReturnSingleMatchWithTokens()
        {
            // Arrange
            var searchService = CreateIndexService();
            var request = new SearchRequest
            {
                TopResults = 10,
                SearchQueryName = "WiseIdPersonSearch",
                SearchFields = new[]
                {
                    new SearchField{ FieldName = "FirstName", Value = "Fred"},
                    new SearchField{ FieldName = "LastName", Value = "Flintstone"},
                },
                SearchConfigurationParameters = new[]
                {
                    new SearchQueryParameterToken{ Name = "WiseIdExactMatchWeight", Value = "3"},
                    new SearchQueryParameterToken{ Name = "WiseIdPartialMatchWeight", Value = ".8"},
                    new SearchQueryParameterToken{ Name = "WiseIdTolerance", Value = ".8"},
                    new SearchQueryParameterToken{ Name = "LocalRowKeyExactMatchWeight", Value = "3"},
                    new SearchQueryParameterToken{ Name = "MinimumFieldMatches", Value = "2"},
                    new SearchQueryParameterToken{ Name = "FirstNameWithSynonymsMatchWeight", Value = "1"},
                    new SearchQueryParameterToken{ Name = "FirstNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken{ Name = "FirstNamePhoneticWeight", Value = ".9"},
                    new SearchQueryParameterToken{ Name = "MiddleNameWithSynonymsMatchWeight", Value = ".5"},
                    new SearchQueryParameterToken{ Name = "MiddleNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken{ Name = "MiddleNamePhoneticWeight", Value = ".4"},
                    new SearchQueryParameterToken{ Name = "LastNameWithSynonymsMatchWeight", Value = "1"},
                    new SearchQueryParameterToken{ Name = "LastNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken{ Name = "BirthDateExactMatchWeight", Value = ".9"},
                    new SearchQueryParameterToken{ Name = "BirthDatePartialMatchWeight", Value = ".75"},
                    new SearchQueryParameterToken{ Name = "BirthDateTolerance", Value = ".7"},
                    new SearchQueryParameterToken{ Name = "GenderExactMatchWeight", Value = ".3"},
                    new SearchQueryParameterToken{ Name = "SuffixExactMatchWeight", Value = ".1"},
                }
            };

            // Act

            var actual = searchService.Search(request);

            // Assert
            actual.Should().NotBeNull();
            //actual.Messages.FirstOrDefault().Should().BeEmpty();
            actual.Success.Should().BeTrue();
            actual.SearchResults.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Ignore]
        [Test]
        public void BatchSearch_ReturnsMultipleMatches()
        {
            // Arrange
            var searchService = CreateIndexService();
            var request = new BatchSearchRequest
            {
                ReferenceId = "batch",
                TopResults = 10,
                SearchQueryName = "WiseIdPersonSearch",
                SearchConfigurationParameters = new[]
                {
                    new SearchQueryParameterToken {Name = "WiseIdExactMatchWeight", Value = "3"},
                    new SearchQueryParameterToken {Name = "WiseIdPartialMatchWeight", Value = ".8"},
                    new SearchQueryParameterToken {Name = "WiseIdTolerance", Value = ".8"},
                    new SearchQueryParameterToken {Name = "LocalRowKeyExactMatchWeight", Value = "3"},
                    new SearchQueryParameterToken {Name = "MinimumFieldMatches", Value = "2"},
                    new SearchQueryParameterToken {Name = "FirstNameWithSynonymsMatchWeight", Value = "1"},
                    new SearchQueryParameterToken {Name = "FirstNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken {Name = "FirstNamePhoneticWeight", Value = ".9"},
                    new SearchQueryParameterToken {Name = "MiddleNameWithSynonymsMatchWeight", Value = ".5"},
                    new SearchQueryParameterToken {Name = "MiddleNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken {Name = "MiddleNamePhoneticWeight", Value = ".4"},
                    new SearchQueryParameterToken {Name = "LastNameWithSynonymsMatchWeight", Value = "1"},
                    new SearchQueryParameterToken {Name = "LastNameTolerance", Value = ".75"},
                    new SearchQueryParameterToken {Name = "BirthDateExactMatchWeight", Value = ".9"},
                    new SearchQueryParameterToken {Name = "BirthDatePartialMatchWeight", Value = ".75"},
                    new SearchQueryParameterToken {Name = "BirthDateTolerance", Value = ".7"},
                    new SearchQueryParameterToken {Name = "GenderExactMatchWeight", Value = ".3"},
                    new SearchQueryParameterToken {Name = "SuffixExactMatchWeight", Value = ".1"},
                },
                Searches = new List<Domain.Search.Search>
                {
                    new Domain.Search.Search
                    {
                        ReferenceId = "1",
                        SearchFields = new[]
                        {
                            new SearchField {FieldName = "FirstName", Value = "Fred"},
                            new SearchField {FieldName = "LastName", Value = "Flintstone"},
                        }
                    },
                    new Domain.Search.Search
                    {
                        ReferenceId = "2",
                        SearchFields = new[]
                        {
                            new SearchField {FieldName = "FirstName", Value = "Jeff"},
                            new SearchField {FieldName = "LastName", Value = "Jacka"},
                        }
                    },
                },
            };

            // Act

            var actual = searchService.Search(request);

            // Assert
            actual.Should().NotBeNull();
            actual.ReferenceId.Should().Be("batch");
            actual.Success.Should().BeTrue();
            actual.SearchResults.Count().Should().Be(2);
            actual.SearchResults.First().ReferenceId.Should().Be("1");
            actual.SearchResults.First().SearchResults.Count.Should().BeGreaterOrEqualTo(1);
            actual.SearchResults.Last().ReferenceId.Should().Be("2");
            actual.SearchResults.Last().SearchResults.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public void Index_ShouldReturnSuccessfully()
        {
            // Arrange
            var indexService = CreateIndexService();
            var request = CreateIndexRequest();

            // Act
            var actual = indexService.Put(request);

            // Assert
            actual.Should().NotBeNull();
            //actual.Messages.FirstOrDefault().Should().BeEmpty();
            actual.Success.Should().BeTrue();
        }

        [Test]
        public void Delete_ShouldRemoveMatch()
        {
            // Arrange
            var searchService = CreateIndexService();
            var searchRequest = new SearchRequest
            {
                TopResults = 10,
                SearchQueryName = "TestSearch",
                SearchFields = new[]
                {
                    new SearchField{ FieldName = "Id", Value = "1"},
                }
            };

            var deleteRequest = new DeleteIndexRequest
            {
                IndexName = "TestSearch",
                IdFieldName = "Id",
                DeleteIds = new[] {"1"}
            };

            // Act
            CreateOrUpdateIndex();
            var searchResponse = searchService.Search(searchRequest);

            if (searchResponse.SearchResults.Count != 1)
            {
                throw new InconclusiveException("Unable to determine if delete will work.  There are " + searchResponse.SearchResults.Count + " matches were 1 was expected");
            }

            var actual = searchService.Delete(deleteRequest);


            // Assert
            actual.Should().NotBeNull();
            actual.Success.Should().BeTrue();

            searchResponse = searchService.Search(searchRequest);
            searchResponse.SearchResults.Should().BeEmpty();
        }


        [Test]
        public void Clear_ShouldRemoveAllIndexes()
        {
            // Arrange
            var searchService = CreateIndexService();
            var searchRequest = new SearchRequest
            {
                TopResults = 10,
                SearchQueryName = "TestSearch",
                SearchFields = new[]
                {
                    new SearchField{ FieldName = "Id", Value = "1"},
                }
            };

            var deleteRequest = new ClearIndexesRequest
            {
                IndexName = "TestSearch",
            };

            // Act
            var actual = searchService.Clear(deleteRequest);


            // Assert
            actual.Should().NotBeNull();
            actual.Success.Should().BeTrue();

            var searchResponse = searchService.Search(searchRequest);
            searchResponse.SearchResults.Should().BeNullOrEmpty();
        }

        [Test]
        public void Optimize_ShouldReturnSuccessful()
        {
            // Arrange
            var searchService = CreateIndexService();
            var optimizeRequest = new OptimizeIndexRequest
            {
                IndexName = "TestSearch",
            };

            // Act
            var actual = searchService.Optimize(optimizeRequest);


            // Assert
            actual.Should().NotBeNull();
            actual.Success.Should().BeTrue();
        }

        private IndexRequest CreateIndexRequest()
        {
            return new IndexRequest
            {
                IndexName = "TestSearch",
                IdFieldName = "Id",
                IndexRecords = new[]
                {
                    new IndexDocument
                    {
                        Fields = new []
                        {
                            new IndexField("Id", "1", isStored: true, isAnalyzed: false),
                            new IndexField("WiseId", "1234", isStored: true, isAnalyzed: true),
                            new IndexField("LocalRowKeys", "abc123", isStored: true, isAnalyzed: true),
                            new IndexField("FirstNames", "Fred", isStored: true, isAnalyzed: true, useTermVector: true),
                            new IndexField("PhoneticFirstNames", "frt", isStored: true, isAnalyzed: true),
                            new IndexField("MiddleNames", null, isStored: false, isAnalyzed: true, useTermVector: true),
                            new IndexField("PhoneticMiddleNames", null, isStored: true, isAnalyzed: true),
                            new IndexField("LastNames", "Flintstone", isStored: false, isAnalyzed: true, useTermVector: true),
                            new IndexField("PhoneticLastNames", "flntstn", isStored: true, isAnalyzed: true),
                            new IndexField("SuffixId", null, isStored: true, isAnalyzed: false),
                            new IndexField("Gender", null, isStored: true, isAnalyzed: false),
                            new IndexField("BirthDate", new DateTime(1981, 4, 16), true)
                        }
                    }
                }

            };
        }
    }
}
