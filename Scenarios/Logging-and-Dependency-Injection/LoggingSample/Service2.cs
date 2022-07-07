using CoreWCF;
using System;
using System.Runtime.Serialization;

namespace LoggingSample
{
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    // The class is marked as partial, this enables CoreWCF to generate methods for injected parameters
    public partial class Service2 : IService2
    {
        // The [Injected] parameter will be created through Dependency Injection and passed in when the method is called
        // CoreWCF will fulfill the interface contract by  generating a matching method that does the DI lookup and calls into the extended version
        public CompositeType GetDataUsingDataContract(CompositeType composite, [Injected] ILogger<Service2> localLogger)
        {
            localLogger.LogInformation("GetDataUsingDataContract called with composite object:  BoolValue:{BoolValue}, StringValue:\"{StringValue}\"", composite.BoolValue, composite.StringValue);
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                // Note: This is skipped by default because of the level set in appsettings.json
                localLogger.LogTrace("StringValue is being modified");
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
