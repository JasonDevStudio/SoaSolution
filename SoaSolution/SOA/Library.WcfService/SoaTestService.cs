using Library.Common;
using Library.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Library.WcfService
{
    [SerializableAttribute]
    public class SoaTestService : ISoaTestService
    {
        [SerializableAttribute]
        delegate object OperateDelegate(out string resultMsg,object value);
          
        public byte[] Operate(out string resultMsg, byte[] bytes)
        { 
            resultMsg = string.Empty;
            byte[] returnObj = null;
            OperateClass ope = (OperateClass)SerializerDeserialize.Deserialize(bytes);

            if (ope == null)
            {
                return returnObj; 
            }

            if (string.IsNullOrWhiteSpace(ope.Assembly))
            {
                return returnObj;
            }

            if (string.IsNullOrWhiteSpace(ope.Class))
            {
                return returnObj;
            }

            if (string.IsNullOrWhiteSpace(ope.Method))
            {
                return returnObj;
            }
             
            Assembly myAssembly = Assembly.Load(ope.Assembly); 
            Type myType = myAssembly.GetType(ope.Class);
            var myTypeInstance = System.Activator.CreateInstance(myType);
            object[] myPara = new object[] { resultMsg,ope.Criteria };

            MethodInfo myMethodInfo = myType.GetMethod(ope.Method, new Type[] { typeof(string).MakeByRefType(), typeof(object) });
            var obj = myMethodInfo.Invoke(myTypeInstance, myPara);

            returnObj = SerializerDeserialize.Serializer(obj);

            return returnObj;
        } 
    }
}
