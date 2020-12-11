using System.Collections.Generic;

namespace WISEsearch.Web.Api.Domain
{
    public class BaseResponse
    {
        public bool Success { get; set; }

        public List<string> Messages { get; set; }

        public BaseResponse()
        {
            Messages = new List<string>();
        }

        //public virtual string ToSerialize()
        //{
        //    string txXML;
        //    using (var ms = new MemoryStream())
        //    {
        //        var ser = new DataContractSerializer(GetType());
        //        ser.WriteObject(ms, this);
        //        ms.Position = 0;
        //        var sr = new StreamReader(ms);
        //        txXML = sr.ReadToEnd();
        //        sr.Close();
        //    }
        //    return txXML;
        //}
    }
}