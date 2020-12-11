using System;
using System.Collections.Generic;
using System.Linq;
using EDFI.Modules.Search.Extensions;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using WISEsearch.Web.Api.Domain.Index;

namespace EDFI.Modules.Search.Lucene
{
    /// <summary>
    /// This class will help the NRTManager track references to a lucene index, allowing it to reopen
    /// index readers when necessary. It also provides an abstraction over all the Lucene operations
    /// </summary>
    public interface ILuceneSearcherContext : IDisposable
    {
        IEnumerable<LuceneSearchResult> Search(Query query, int topResults, bool explain = false);
        void CreateOrUpdateIndex(string idFieldName, IEnumerable<LuceneDocument> documents);
        void DeleteIndexes(string fieldName, string fieldValue);
        void DeleteIndexes(string fieldName, IEnumerable<string> fieldValues);
        void ClearIndexes();
        void Optimize();

        LuceneSearchResult GetDocument(int docId);
        IEnumerable<LuceneSearchResult> GetFirstResults(int topResults);
        int GetDocumentCount();
    }

    public class LuceneSearcherContext : ILuceneSearcherContext
    {
        private readonly LuceneIndexContext _luceneIndexContext;

        public LuceneSearcherContext(LuceneIndexContext luceneIndexContext)
        {
            _luceneIndexContext = luceneIndexContext;

            SearcherReference = luceneIndexContext.Manager.GetSearcherManager().Acquire();
        }

        protected IndexWriter Writer { get { return _luceneIndexContext.Writer; } }
        protected NrtManager Manager { get { return _luceneIndexContext.Manager; } }
        protected SearcherManager.IndexSearcherToken SearcherReference { get; set; }
        protected Searcher Searcher { get { return SearcherReference.Searcher; } }

        public int GetDocumentCount()
        {
            return SearcherReference.Searcher.IndexReader.NumDocs();
        }

        public LuceneSearchResult GetDocument(int docId)
        {
            var document = SearcherReference.Searcher.IndexReader.Document(docId);

            return new LuceneSearchResult
            {
                IndexDocumentId = docId,
                Fields =
                    document
                        .GetFields()
                        .ToDictionary(x => x.Name, x => x.StringValue)
            };
        }

        public IEnumerable<LuceneSearchResult> GetFirstResults(int topResults)
        {
            var results = Searcher.Search(new MatchAllDocsQuery(), topResults);

            return results.ScoreDocs
                .Select(
                    match =>
                        new LuceneSearchResult
                        {
                            IndexDocumentId = match.Doc,
                            Fields =
                                Searcher.Doc(match.Doc)
                                    .GetFields()
                                    .ToDictionary(x => x.Name, x => x.StringValue)
                        })
                .ToList();
        }

        public IEnumerable<LuceneSearchResult> Search(Query query, int topResults,
            bool explain = false)
        {
            var results = Searcher.Search(query, topResults);

            return results.ScoreDocs
                .Select(
                    match =>
                        new LuceneSearchResult
                        {
                            IndexDocumentId = match.Doc,
                            Fields =
                                Searcher.Doc(match.Doc)
                                    .GetFields()
                                    .ToDictionary(x => x.Name, x => x.StringValue),
                            Explanation = explain ? Searcher.Explain(query, match.Doc).ToHtml() : null,
                            Score = (decimal) match.Score
                        })
                .ToList();
        }

        public void CreateOrUpdateIndex(string idFieldName, IEnumerable<LuceneDocument> documents)
        {

            foreach (var doc in documents)
            {
                var document = new Document();
                foreach (var field in doc.Fields)
                {
                    document.Add(CreateLuceneField(field));
                }
                var id = doc.Fields.Single(x => x.Name == idFieldName).Value;
                Manager.UpdateDocument(new Term(idFieldName, id), document);
            }

            //Writer.Commit();
            Manager.MaybeReopen(false);
        }

        private IFieldable CreateLuceneField(LuceneField field)
        {
            var isStored = field.IsStored ? Field.Store.YES : Field.Store.NO;
                    var isAnalyzed = field.IsAnalyzed
                        ? Field.Index.ANALYZED_NO_NORMS
                        : Field.Index.NOT_ANALYZED_NO_NORMS;
                    var termVector = field.UseTermVector ? Field.TermVector.YES : Field.TermVector.NO;

            switch (field.DataType)
            {
                case LuceneFieldDataType.Long: 
                    
                    var numericStorageValue =
                        field.Value.ParseLongOrNull().ErrorIfNull(string.Format("Unable to convert string '{0}' to Numeric Field", field.Value));
                    var numericField = new NumericField(field.Name, 0, isStored, true);

                    numericField.SetLongValue(numericStorageValue);
                    return numericField;

                case LuceneFieldDataType.Date:
                    var dateStorageValue =
                        field.Value.ParseIntOrNull().ErrorIfNull(string.Format("Unable to convert date string '{0}' to numeric", field.Value));
                    var luceneField = new NumericField(field.Name, isStored, true);

                    luceneField.SetIntValue(dateStorageValue);

                    return luceneField;
                case LuceneFieldDataType.DateTime:
                    
                    var dateTimeStorageValue =
                        field.Value.ParseLongOrNull().ErrorIfNull(string.Format("Unable to convert date string '{0}' to numeric", field.Value));
                    var dateTimeField = new NumericField(field.Name, isStored, true);

                    dateTimeField.SetLongValue(dateTimeStorageValue);

                    return dateTimeField;
                case LuceneFieldDataType.String:
                default:
                    return new Field(field.Name, field.Value ?? String.Empty, isStored, isAnalyzed, termVector);
            }
        }

        public void DeleteIndexes(string fieldName, string fieldValue)
        {
            Manager.DeleteDocuments(new Term(fieldName, fieldValue));

            //Writer.Commit();
            Manager.MaybeReopen(true);
        }

        public void DeleteIndexes(string fieldName, IEnumerable<string> fieldValues)
        {
            foreach (var fieldValue in fieldValues)
            {
                Manager.DeleteDocuments(new Term(fieldName, fieldValue));
            }

            //Writer.Commit();
            Manager.MaybeReopen(true);
        }

        public void ClearIndexes()
        {
            Manager.DeleteAll();

            Writer.Commit();
            Manager.MaybeReopen(true);
        }

        public void Dispose()
        {

            try
            {
                Writer.Commit();
                SearcherReference.Dispose();
            }
            catch { }
        }

        public void Optimize()
        {
            Writer.Commit();
            Writer.Optimize();

            Manager.MaybeReopen(false);
        }
    }
}