using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using Lucene.Net.Store;

namespace EDFI.Modules.Search.Lucene
{
    /// <summary>
    /// This factory is needs to be treated as a singleton to keep any needed LuceneIndexContext open
    /// until the application shuts down for optimium performance
    /// </summary>
    public interface ILuceneSearcherContextFactory
    {
        ILuceneSearcherContext CreateSearchContext(string targetIndex);
    }

    public class LuceneSearcherContextFactory : ILuceneSearcherContextFactory, IDisposable
    {
        ~LuceneSearcherContextFactory()
        {
            Dispose();
        }

        private readonly object _lockObject = new object();
        private readonly string _indexDirectory;
        private readonly Dictionary<string, LuceneIndexContext> _indexes = new Dictionary<string, LuceneIndexContext>(StringComparer.InvariantCultureIgnoreCase);
        private Timer _commitChangesTimer;
        private Timer _optimizeTimer;
        private DateTime? _lastAccessed;
        private bool _optimizing;

        public LuceneSearcherContextFactory()
        {
            _indexDirectory = ConfigurationManager.AppSettings["EDFI.Modules.Search.IndexBaseDirectory"];
            _commitChangesTimer = new Timer(OnCommitChanges, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            //_optimizeTimer = new Timer(OnOptimize, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
        }

        public ILuceneSearcherContext CreateSearchContext(string targetIndex)
        {
            _lastAccessed = DateTime.Now;
            if (_indexes.ContainsKey(targetIndex) == false)
            {
                lock (_lockObject)
                {
                    if (_indexes.ContainsKey(targetIndex) == false)
                    {
                        var directoryPath = Path.Combine(_indexDirectory, targetIndex);

                        var writeLock = Path.Combine(directoryPath, "write.lock");
                        if (File.Exists(writeLock))
                        {
                            File.Delete(writeLock);
                        }

                        var directory = FSDirectory.Open(new DirectoryInfo(directoryPath));
                        var indexContext = new LuceneIndexContext(directory);
                        _indexes.Add(targetIndex, indexContext);
                    }
                }
            }

            var searcherContext = new LuceneSearcherContext(_indexes[targetIndex]);
            return searcherContext;
        }


        private void OnCommitChanges(object state)
        {
            foreach (var luceneIndexContext in _indexes.Values)
            {
                luceneIndexContext.Writer.Commit();
            }
        }

        private void OnOptimize(object state)
        {
            // try to avoid optimizing during high access periods
            if (_optimizing || (_lastAccessed.HasValue && DateTime.Now.Subtract(_lastAccessed.Value).TotalMinutes > 5))
            {
                return;
            }

            _optimizing = true;

            foreach (var luceneIndexContext in _indexes.Values)
            {
                luceneIndexContext.Writer.Optimize();
            }

            _optimizing = false;
        }

        public void Dispose()
        {
            _commitChangesTimer.Dispose();
            //_optimizeTimer.Dispose();
            foreach (var index in _indexes.Values)
            {
                index.Dispose();
            }
        }
    }
}