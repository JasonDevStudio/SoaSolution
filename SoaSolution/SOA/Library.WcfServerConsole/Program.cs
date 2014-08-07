using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Library.WcfServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Uri[] baseAddresses = new Uri[] { 
                  new Uri("net.tcp://localhost:8080/Design_Time_Addresses/ConsoleApplication1/Service1/")
              };

                ServiceHost host = new ServiceHost(typeof(Library.WcfService.SoaTestService));
                host.Open();

                Console.WriteLine("服务已启动！");
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                Console.WriteLine("服务启动失败：" + ex.Message);
                Console.ReadKey();

            }
        }
    }
}
