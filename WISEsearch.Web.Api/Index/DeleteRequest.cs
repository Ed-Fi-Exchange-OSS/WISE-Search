using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WISEsearch.Web.Api.Index
{ [DataContract]
    public class DeleteRequest : BaseRequest
    {
         [DataMember]
        public string FieldName { get; set; }
         [DataMember]
        public IEnumerable<string> FieldValues { get; set; }
    }
}