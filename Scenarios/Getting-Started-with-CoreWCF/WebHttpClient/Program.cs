using WebHttpClient;

// Use a single HttpClient instance for multiple requests as it will pool connections 
var httpClient = new HttpClient();

// Using a wrapper generated based on the service OpenAPI definition
var client = new WebAPIGeneratedWrapper("http://localhost:8080/", httpClient);

// Calls /api/path/{param}
string result = await client.PathAsync("Testing_the_path_endpoint");
Console.WriteLine(result);

// Calls /api/query?param=value
result = await client.QueryAsync("Testing the query endpoint");
Console.WriteLine(result);

// Calls /api/body with a complex data structure
var data = CreateExampleContract();
var result2 = await client.BodyAsync(data);
Console.WriteLine(JsonSerialize(result2));

ExampleContract CreateExampleContract()
{
    var contract = new WebHttpClient.ExampleContract()
    {
        SimpleProperty = "House Stark",
        ComplexProperty = new WebHttpClient.ExampleContractInnerExampleResponse { Name = "Jon Snow" },
        SimpleCollection = "Winter is coming".Split(" ")
    };

    var inner = new ExampleContractArrayInnerExampleResponse() { Name = "Arya Stark" };
    var inner2 = new ExampleContractArrayInnerExampleResponse() { Name = "Sansa Stark" };

    contract.ComplexCollection = new System.Collections.Generic.List<ExampleContractArrayInnerExampleResponse>(new ExampleContractArrayInnerExampleResponse[] { inner, inner2 });
    return contract;
}

string JsonSerialize<T>(T thing)
{
    var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
    var sw = new StringWriter();
    var writer = new Newtonsoft.Json.JsonTextWriter(sw);
    writer.Formatting = Newtonsoft.Json.Formatting.Indented;
    jsonSerializer.Serialize(writer, thing);
    return sw.ToString();
}

