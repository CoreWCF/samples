namespace Service;

[ServiceContract]
public interface ISecuredService
{
    [OperationContract]
    string Echo(string value);
}
