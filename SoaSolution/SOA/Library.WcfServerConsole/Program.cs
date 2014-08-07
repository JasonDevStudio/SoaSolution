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
                ServiceHost host = new ServiceHost(typeof(WcfService.SoaTestService));
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
