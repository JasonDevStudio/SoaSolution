﻿using Library.Common;
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
        delegate object OperateDelegate(out string resultMsg,object value);
         
        [OperationContract]
        public object Operate(out string resultMsg, byte[] bytes)
        { 
            resultMsg = string.Empty;
            object returnObj = null;
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
            MethodInfo myMethodInfo = myType.GetMethod(ope.Method, new Type[] { typeof(string).MakeByRefType(), typeof(object) });
            OperateDelegate method = (OperateDelegate)Delegate.CreateDelegate(myType, myMethodInfo);
            returnObj = method(out resultMsg ,ope.Criteria);

            return returnObj;
        }
         
    }
}
