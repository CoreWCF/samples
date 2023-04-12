﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreWcf.Samples.TokenProvider
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://CoreWcf.Samples.TokenProvider", ConfigurationName="CoreWcf.Samples.TokenProvider.ICalculatorService")]
    public interface ICalculatorService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Add", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/AddResponse")]
        double Add(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Add", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/AddResponse")]
        System.Threading.Tasks.Task<double> AddAsync(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Subtract", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/SubtractResponse")]
        double Subtract(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Subtract", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/SubtractResponse")]
        System.Threading.Tasks.Task<double> SubtractAsync(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Multiply", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/MultiplyResponse")]
        double Multiply(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Multiply", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/MultiplyResponse")]
        System.Threading.Tasks.Task<double> MultiplyAsync(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Divide", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/DivideResponse")]
        double Divide(double n1, double n2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://CoreWcf.Samples.TokenProvider/ICalculatorService/Divide", ReplyAction="http://CoreWcf.Samples.TokenProvider/ICalculatorService/DivideResponse")]
        System.Threading.Tasks.Task<double> DivideAsync(double n1, double n2);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface ICalculatorServiceChannel : CoreWcf.Samples.TokenProvider.ICalculatorService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class CalculatorServiceClient : System.ServiceModel.ClientBase<CoreWcf.Samples.TokenProvider.ICalculatorService>, CoreWcf.Samples.TokenProvider.ICalculatorService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CalculatorServiceClient() : 
                base(CalculatorServiceClient.GetDefaultBinding(), CalculatorServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WSHttpBinding_ICalculatorService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(CalculatorServiceClient.GetBindingForEndpoint(endpointConfiguration), CalculatorServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CalculatorServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CalculatorServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public double Add(double n1, double n2)
        {
            return base.Channel.Add(n1, n2);
        }
        
        public System.Threading.Tasks.Task<double> AddAsync(double n1, double n2)
        {
            return base.Channel.AddAsync(n1, n2);
        }
        
        public double Subtract(double n1, double n2)
        {
            return base.Channel.Subtract(n1, n2);
        }
        
        public System.Threading.Tasks.Task<double> SubtractAsync(double n1, double n2)
        {
            return base.Channel.SubtractAsync(n1, n2);
        }
        
        public double Multiply(double n1, double n2)
        {
            return base.Channel.Multiply(n1, n2);
        }
        
        public System.Threading.Tasks.Task<double> MultiplyAsync(double n1, double n2)
        {
            return base.Channel.MultiplyAsync(n1, n2);
        }
        
        public double Divide(double n1, double n2)
        {
            return base.Channel.Divide(n1, n2);
        }
        
        public System.Threading.Tasks.Task<double> DivideAsync(double n1, double n2)
        {
            return base.Channel.DivideAsync(n1, n2);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WSHttpBinding_ICalculatorService))
            {
                System.ServiceModel.WSHttpBinding result = new System.ServiceModel.WSHttpBinding();
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.SecurityMode.TransportWithMessageCredential;
                result.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
                result.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.UserName;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WSHttpBinding_ICalculatorService))
            {
                return new System.ServiceModel.EndpointAddress("https://localhost:5001/CalculatorService");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return CalculatorServiceClient.GetBindingForEndpoint(EndpointConfiguration.WSHttpBinding_ICalculatorService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return CalculatorServiceClient.GetEndpointAddress(EndpointConfiguration.WSHttpBinding_ICalculatorService);
        }
        
        public enum EndpointConfiguration
        {
            
            WSHttpBinding_ICalculatorService,
        }
    }
}
