using System;
using System.ServiceModel;
using Contract;

namespace NetCoreClient
{
    public class Program
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8089;

        static void Main(string[] args)
        {
            Console.Title = "WCF .Net Core Client";

            CallBasicHttpBinding($"http://localhost:{HTTP_PORT}");
            CallBasicHttpBinding($"https://localhost:{HTTPS_PORT}");
            CallWsHttpBinding($"http://localhost:{HTTP_PORT}");
            CallWsHttpBinding($"https://localhost:{HTTPS_PORT}");
            CallNetTcpBinding($"net.tcp://localhost:{NETTCP_PORT}");
        }

        private static void CallBasicHttpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new BasicHttpBinding(IsHttps(hostAddr) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/basicHttp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;

                var result = client.Echo("Hello World!");
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static void CallWsHttpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new WSHttpBinding(IsHttps(hostAddr) ? SecurityMode.Transport : SecurityMode.None);

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/wsHttp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;

                var result = client.Echo("Hello World!");
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static void CallNetTcpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new NetTcpBinding();

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/netTcp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;

                var result = client.Echo("Hello World!");
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static bool IsHttps(string url)
        {
            return url.ToLower().StartsWith("https://");
        }
    }
}
