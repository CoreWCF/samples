using CoreWCF;
using System;
using System.Runtime.Serialization;

namespace LoggingSample
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    public class Service : IService
    {
        private ILogger<Service> _logger;

        // Parameterized constructor will be called by Dependency Injection
        public Service(ILogger<Service> logger)
        {
            _logger = logger;
        }

        public string GetData(int value)
        {
            _logger.LogInformation("GetData called with value {value}", value);
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            _logger.LogInformation("GetDataUsingDataContract called with composite object:  BoolValue:{BoolValue}, StringValue:\"{StringValue}\"", composite.BoolValue, composite.StringValue);
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
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
