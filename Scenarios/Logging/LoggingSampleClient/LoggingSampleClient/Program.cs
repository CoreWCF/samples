// See https://aka.ms/new-console-template for more information
Console.WriteLine("LoggingSampleClient");

//Using a wrapper generated using Add Service Reference in Visual Studio
var client = new ServiceReference1.ServiceClient();

var result = await client.GetDataAsync(new Random().Next(100));
Console.WriteLine($"The Result is {result}");

var obj = new ServiceReference1.CompositeType() { BoolValue = true, StringValue = "Mary had a little lamb" };
var result2 = await client.GetDataUsingDataContractAsync(obj);
Console.WriteLine($"The results are :- BoolValue: {result2.BoolValue}, StringValue: \"{result2.StringValue}\"");