using Library.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Library.WcfService
{ 
    [ServiceContract] 
    public interface ISoaTestService
    {
        /// <summary>
        /// 操作
        /// </summary> 
        [OperationContract]
        byte[] Operate(out string resultMsg, byte[] bytes);
         
    }
}
