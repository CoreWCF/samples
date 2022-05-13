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
    return new ExampleContract()
    {
        SimpleProperty = "House Stark",
        ComplexProperty = new () { Name = "Jon Snow" },
        SimpleCollection = "Winter is coming".Split(" "),
        ComplexCollection = new ExampleContractArrayInnerContract[] { new() { Name = "Arya Stark" }, new() { Name = "Sansa Stark" } }
    };
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

