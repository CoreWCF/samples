using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contract
{
    [DataContract]
    public class EchoFault
    {
        [DataMember]
        public string Text { get; set; }
    }

    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        Task<string> Echo(string text);

        [OperationContract]
        Task<string> ComplexEcho(EchoMessage text);

        [OperationContract]
        [FaultContract(typeof(EchoFault))]
        Task<string> FailEcho(string text);
    }

    [DataContract]
    public class EchoMessage
    {
        [DataMember]
        public string Text { get; set; }
    }
}
