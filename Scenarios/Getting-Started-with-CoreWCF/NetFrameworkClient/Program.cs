using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Contract;

namespace NetFrameworkClient
{

    public class Program
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8089;

        static async Task Main(string[] args)
        {
            Console.Title = "WCF .Net Framework Client";

            await CallBasicHttpBinding($"http://localhost:{HTTP_PORT}");
            await CallBasicHttpBinding($"https://localhost:{HTTPS_PORT}");
            await CallWsHttpBinding($"http://localhost:{HTTP_PORT}");
            await CallWsHttpBinding($"https://localhost:{HTTPS_PORT}");
            await CallNetTcpBinding($"net.tcp://localhost:{NETTCP_PORT}");
        }

        private static async Task CallBasicHttpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new BasicHttpBinding(IsHttps(hostAddr) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/basicHttp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;
                channel.Open();
                var result = await client.Echo("Hello World!");
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static async Task CallWsHttpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new WSHttpBinding(IsHttps(hostAddr) ? SecurityMode.Transport : SecurityMode.None);

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/wsHttp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;
                channel.Open();
                var result = await client.Echo("Hello World!");
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static async Task CallNetTcpBinding(string hostAddr)
        {
            IClientChannel channel = null;

            var binding = new NetTcpBinding();

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/nettcp"));
            factory.Open();
            try
            {
                IEchoService client = factory.CreateChannel();
                channel = client as IClientChannel;
                channel.Open();
                var result = await client.Echo("Hello World!");
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
