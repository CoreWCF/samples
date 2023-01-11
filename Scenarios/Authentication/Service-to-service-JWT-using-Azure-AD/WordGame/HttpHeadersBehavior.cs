using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CoreWCF.Helpers
{
    public class HttpHeadersBehavior : IEndpointBehavior
    {
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public IDictionary<string, string> Headers => _headers;


        // IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime behavior)
        {
            behavior.ClientMessageInspectors.Add(new HttpHeaderInjectionMessageInspector(_headers));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatcher)
        {
            return;
            //endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new EndpointBehaviorMessageInspector());
        }

        public void Validate(ServiceEndpoint serviceEndpoint)
        {
            return;
        }

        public class HttpHeaderInjectionMessageInspector : IClientMessageInspector
        {
            private IDictionary<string, string> _headers;

            public HttpHeaderInjectionMessageInspector(IDictionary<string, string> headers)
            {
                _headers = headers;
            }

            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
            }

            public object? BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                HttpRequestMessageProperty? requestMessage;
                if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    requestMessage = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                }
                else
                {
                    requestMessage = new HttpRequestMessageProperty();
                    request.Properties[HttpRequestMessageProperty.Name] = requestMessage;
                }
                if (requestMessage != null)
                {
                    foreach (var prop in _headers)
                    {
                        requestMessage.Headers[prop.Key] = prop.Value;
                    }
                }
                return null;
            }
        }
    }

    public static class HttpHeadersBehaviorHelpers
    {
        public static void AddRequestHeader<T>(this ClientBase<T> client, string HeaderName, string HeaderValue) where T : class
        {
            var behavior = GetHttpHeadersBehavior(client);
            behavior.Headers[HeaderName] = HeaderValue;
        }

        public static void AddRequestHeaders<T>(this ClientBase<T> client, IDictionary<string, string> headers) where T : class
        {
            var behavior = GetHttpHeadersBehavior(client);
            foreach (var header in headers)
            {
                behavior.Headers[header.Key] = header.Value;
            }
        }

        private static HttpHeadersBehavior GetHttpHeadersBehavior<T>(ClientBase<T> client) where T : class
        {
            HttpHeadersBehavior behavior;

            if (client.Endpoint.EndpointBehaviors.Contains(typeof(HttpHeadersBehavior)))
            {
                behavior = (HttpHeadersBehavior)client.Endpoint.EndpointBehaviors[typeof(HttpHeadersBehavior)];
            }
            else
            {
                behavior = new HttpHeadersBehavior();
                client.Endpoint.EndpointBehaviors.Add(behavior);
            }

            return behavior;
        }
    }
}
