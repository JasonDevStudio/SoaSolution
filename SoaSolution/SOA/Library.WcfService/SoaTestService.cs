using Library.Common;
using Library.Facade.SoaTest;
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
        /// <summary>
        /// 统一反射操作
        /// </summary> 
        public byte[] Operate(byte[] bytes)
        {  
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

            MethodInfo myMethodInfo = myType.GetMethod(ope.Method, new Type[] { typeof(string).MakeByRefType(), typeof(object) });
            ope.ResultObj = myMethodInfo.Invoke(myTypeInstance, ope.Parameters);

            returnObj = SerializerDeserialize.Serializer(ope);

            return returnObj;
        }

        /// <summary>
        /// 类反射操作
        /// </summary> 
        public byte[] FacadeTestUserOperate(byte[] bytes)
        {
            byte[] returnObj = null;
            OperateClass ope = (OperateClass)SerializerDeserialize.Deserialize(bytes);

            if (ope == null)
            {
                return returnObj;
            }
             
            if (string.IsNullOrWhiteSpace(ope.Method))
            {
                return returnObj;
            }

            IFacadeTestUser testUser = new Factories().InstanceTestUser();
            Type myType = testUser.GetType();
            MethodInfo myMethodInfo = myType.GetMethod(ope.Method, new Type[] { typeof(string).MakeByRefType(), typeof(object) });
            ope.ResultObj = myMethodInfo.Invoke(testUser, ope.Parameters);

            returnObj = SerializerDeserialize.Serializer(ope);

            return returnObj;
        }
    }
}
