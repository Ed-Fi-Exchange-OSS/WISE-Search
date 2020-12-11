using System;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace EDFI.Modules.Search.Lucene
{
    /// <summary>
    /// This class represents a Lucene index for a Lucene directory.  
    /// It will create a write lock until it is disposed of so there should only ever be one of these 
    /// for a Lucene directory
    /// </summary>
    public class LuceneIndexContext : IDisposable
    {
        public LuceneIndexContext(Directory directory)
        {
            Analyzer = new WhitespaceAnalyzer();
            Writer = new IndexWriter(directory, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            Manager = new NrtManager(Writer);
        }

        public NrtManager Manager { get; set; }
        public IndexWriter Writer { get; set; }
        public Analyzer Analyzer { get; set; }

        public void Dispose()
        {
            try
            {
                Analyzer.Dispose();
            }
            catch { }

            try
            {
                Manager.Dispose();
            }
            catch { }

            try
            {
                Writer.Dispose();
            }
            catch { }
        }
    }
}