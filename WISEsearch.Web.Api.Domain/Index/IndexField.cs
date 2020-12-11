using System;

namespace WISEsearch.Web.Api.Domain.Index
{
    public class IndexField
    {
        public IndexField(){}
        public IndexField(string name, string value, bool isAnalyzed = true, bool isStored = false,
            bool useTermVector = false, LuceneFieldDataType dataType = LuceneFieldDataType.String)
        {
            Name = name;
            Value = value;
            IsAnalyzed = isAnalyzed;
            IsStored = isStored;
            UseTermVector = useTermVector;
            DataType = dataType;
        }

        public IndexField(string name, DateTime value, bool dateOnly = true) 
            : this(name, null, false, true, false, LuceneFieldDataType.Date)
        {
            if (dateOnly)
            {
                Value = value.ToString("yyyyMMdd");
                DataType = LuceneFieldDataType.Date;
            }
            else
            {
                Value = value.ToString("yyyyMMddHHmmssfff");
                DataType = LuceneFieldDataType.DateTime;
            }
        }

        public IndexField(string name, long value)
            : this(name, null, false, true, false, LuceneFieldDataType.Long)
        {
            Value = value.ToString();
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsStored { get; set; }

        public bool UseTermVector { get; set; }
        public LuceneFieldDataType DataType { get; set; }

        public bool IsAnalyzed { get; set; }

    }
}