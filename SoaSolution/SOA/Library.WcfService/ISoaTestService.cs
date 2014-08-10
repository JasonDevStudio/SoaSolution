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
        /// 统一反射操作
        /// </summary> 
        [OperationContract]
        byte[] Operate(byte[] bytes);

        /// <summary>
        /// 类反射操作
        /// </summary> 
        [OperationContract]
        byte[] FacadeTestUserOperate(byte[] bytes);
         
    }
}
