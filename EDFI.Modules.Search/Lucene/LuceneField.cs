using System;
using WISEsearch.Web.Api.Domain.Index;

namespace EDFI.Modules.Search.Lucene
{
    public class LuceneField
    {
        public LuceneField(string name, string value, bool isAnalyzed = true, bool isStored = false,
            bool useTermVector = false, LuceneFieldDataType dataType = LuceneFieldDataType.String)
        {
            Name = name;
            Value = value;
            IsAnalyzed = isAnalyzed;
            IsStored = isStored;
            UseTermVector = useTermVector;
            DataType = dataType;
        }

        public LuceneField(string name, long value, bool isAnalyzed = true, bool isStored = false, bool useTermVector = false)
            : this(name, value.ToString(), isAnalyzed, isStored, useTermVector, LuceneFieldDataType.Long)
        {
        }

        public LuceneField(string name, DateTime value, bool isAnalyzed = true, bool isStored = false, bool useTermVector = false)
            : this(name, value.ToString(), isAnalyzed, isStored, useTermVector, LuceneFieldDataType.Long)

        {
        }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public bool IsStored { get; private set; }

        public bool UseTermVector { get; set; }
        public LuceneFieldDataType DataType { get; set; }

        public bool IsAnalyzed { get; private set; }
    }
}