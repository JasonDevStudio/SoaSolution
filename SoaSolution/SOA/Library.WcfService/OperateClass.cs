using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Library.WcfService
{
    [SerializableAttribute]
    [DataContract]
    public class OperateClass
    {
        [DataMember]
        public string Assembly { get; set; }

        [DataMember]
        public string Class { get; set; } 

        [DataMember]
        public string Method { get; set; }
        
        [DataMember]
        public object Criteria { get; set; }
    }
}
