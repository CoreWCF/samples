namespace CoreWcf.Samples.GettingStarted
{
    // Define a service contract.
    [ServiceContract(Namespace = "http://CoreWcf.Samples.GettingStarted")]
    public interface ICalculatorService
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }
}
