## Web Http Client Example

This sample uses the built-in NSWAG support in Visual Studio to create a client wrapper for the service documented using OpenAPI attributes.

### Referencing an OpenAPI service in Visual Studio

* Make sure the server is running if you are going to use it to generate a swagger doc
* Right click on the project and choose the Add -> Connected Service option
* On the connected services page, Click the "Add a service Reference" link
* In the resulting dialog, choose the "OpenAPI" option
* Supply the URL for the *OpenAPI JSON*. 
    * This is different from the test pages created, but is linked near the top of the page
    * The default Endpoint for `UseSwagger()` is `/swagger`, eg http://localhost:8080/swagger which will open the test page
* That will download the API definition JSON to the project
* Build the project to generate the API wrapper from the Swagger
* The easiest way to see the resulting code is through:
    * The "View generated code" menu option from the "..." in the connected services page
    * Class View (Ctrl + Shift + C)
    * Object Browser (Ctrl + Alt + J)
    * Use the classname you used above via intellisense, and then F12 to navigate to the code 

### Updating the Service definitions

* The client definition will not automatically update based on the server changes, you need to manually refresh
* Right click on the project and choose the Add -> Connected Service option to show the connected services page
* Use the "..." option for the connected service to show a menu and choose "Refresh"
* Build the project to regenerate the client wrapper.
