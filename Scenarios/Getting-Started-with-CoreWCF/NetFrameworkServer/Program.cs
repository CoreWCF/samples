using System;
using System.Linq;
using System.ServiceModel;

namespace DesktopServer
{
    class Program
    {
        static void Main()
        {
            var httpUrl = "http://localhost:8088";
            var httpsUrl = "https://localhost:8443";
            var netTcpUrl = "net.tcp://localhost:8089";

            Uri[] baseUriList = new Uri[] { new Uri(httpUrl), new Uri(httpsUrl), new Uri(netTcpUrl) };

            Type contract = typeof(Contract.IEchoService);
            var host = new ServiceHost(typeof(EchoService), baseUriList);

            host.AddServiceEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None), "/basichttp");
            host.AddServiceEndpoint(contract, new BasicHttpsBinding(BasicHttpsSecurityMode.Transport), "/basichttp");
            host.AddServiceEndpoint(contract, new WSHttpBinding(SecurityMode.None), "/wsHttp");
            host.AddServiceEndpoint(contract, new WSHttpBinding(SecurityMode.Transport), "/wsHttp");
            host.AddServiceEndpoint(contract, new NetTcpBinding(), "/nettcp");

            host.Open();

            LogHostUrls(host);

            Console.WriteLine("Hit enter to close");
            Console.ReadLine();

            host.Close();
        }

           private static void LogHostUrls(ServiceHost host)
        {
            foreach (System.ServiceModel.Description.ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening on " + endpoint.ListenUri.ToString());
            }
        }

    }
}
