// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace WebHttp
{
    // Note the OpenAPI attributes are not strictly necessary as defaults are chosen in most cases,
    // but are used here to show how they can customize the swagger documentation.

    [ServiceContract]
    [OpenApiBasePath("/api")]
    internal interface IWebApi
    {
        [OperationContract]
        [WebGet(UriTemplate = "/path/{param}")]
        [OpenApiTag("Tag")]
        [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(string))]
        string PathEcho(
            [OpenApiParameter(Description = "param description.")] string param);

        [OperationContract]
        [WebGet(UriTemplate = "/query?param={param}")]
        [OpenApiTag("Tag")]
        [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(string))]
        string QueryEcho(
            [OpenApiParameter(Description = "param description.")] string param = "");

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/body")]
        [OpenApiTag("Tag")]
        [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(ExampleContract)) ]
        ExampleContract BodyEcho(
            [OpenApiParameter(ContentTypes = new[] { "application/json", "text/xml" }, Description = "param description.")] ExampleContract param);
    }
}
