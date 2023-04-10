﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreWcf.Samples.CompressionMessageEncoder
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CoreWcf.Samples.CompressionMessageEncoder.IEchoService")]
    public interface IEchoService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEchoService/Echo", ReplyAction="http://tempuri.org/IEchoService/EchoResponse")]
        string Echo(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEchoService/Echo", ReplyAction="http://tempuri.org/IEchoService/EchoResponse")]
        System.Threading.Tasks.Task<string> EchoAsync(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEchoService/BigEcho", ReplyAction="http://tempuri.org/IEchoService/BigEchoResponse")]
        string[] BigEcho(string[] input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEchoService/BigEcho", ReplyAction="http://tempuri.org/IEchoService/BigEchoResponse")]
        System.Threading.Tasks.Task<string[]> BigEchoAsync(string[] input);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface IEchoServiceChannel : CoreWcf.Samples.CompressionMessageEncoder.IEchoService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class EchoServiceClient : System.ServiceModel.ClientBase<CoreWcf.Samples.CompressionMessageEncoder.IEchoService>, CoreWcf.Samples.CompressionMessageEncoder.IEchoService
    {
        
        public EchoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string Echo(string input)
        {
            return base.Channel.Echo(input);
        }
        
        public System.Threading.Tasks.Task<string> EchoAsync(string input)
        {
            return base.Channel.EchoAsync(input);
        }
        
        public string[] BigEcho(string[] input)
        {
            return base.Channel.BigEcho(input);
        }
        
        public System.Threading.Tasks.Task<string[]> BigEchoAsync(string[] input)
        {
            return base.Channel.BigEchoAsync(input);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
    }
}
