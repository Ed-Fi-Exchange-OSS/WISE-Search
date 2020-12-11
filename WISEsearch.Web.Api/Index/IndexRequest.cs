using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WISEsearch.Web.Api.Index
{
    [DataContract]
    public class IndexRequest : BaseRequest
    {
        [DataMember]
        public IEnumerable<LuceneDocument> Documents { get; set; }
   
    }
}