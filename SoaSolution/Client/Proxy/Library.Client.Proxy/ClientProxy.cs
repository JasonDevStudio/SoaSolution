using Library.Client.Proxy.SoaTestService;
using Library.Common;
using Library.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Library.Client.Proxy
{
    public class ClientProxy
    {
        ChannelFactory<SoaTestService.ISoaTestService> factory = new ChannelFactory<SoaTestService.ISoaTestService>("NetTcpBinding_ISoaTestService");
        public OperateClass Operate(OperateClass opc)
        {
            var clent = factory.CreateChannel();
            var bytes = SerializerDeserialize.Serializer(opc);
            var resultBytes = clent.Operate(bytes);
            var resultOpe = (OperateClass)SerializerDeserialize.Deserialize(resultBytes);

            return resultOpe;
        }

        public OperateClass FacadeTestUserOperate(OperateClass opc)
        {
            var clent = factory.CreateChannel();
            var bytes = SerializerDeserialize.Serializer(opc);
            var resultBytes = clent.FacadeTestUserOperate(bytes);
            var resultOpe = (OperateClass)SerializerDeserialize.Deserialize(resultBytes);

            return resultOpe;
        }
    }
}
