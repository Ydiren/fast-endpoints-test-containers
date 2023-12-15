# FastEndpoints and TestContainers
## Description

A small project to demonstrate the usage of both FastEndpoints and Test Containers in the same solution.
## [FastEndpoints](https://fast-endpoints.com/)<br/>
FastEndpoints uses the REPR pattern (Request-EndPoint-Response) to keep all classes/files related to the endpoint in close proximity (usually all within the same file)

Out of the box, FastEndpoints provides patterns for easily implementing Endpoints, validators and mappers. Using the provided snippets it becomes really easy to generate a new endpoint, all of the boiler plate is done for you!
#### FastEndpoint Resources
[Visual Studio Snippets](https://fast-endpoints.com/docs/scaffolding#feature-scaffolding)<br/>
[Rider Snippets](https://gist.github.com/dj-nitehawk/6493cb85bf3bb20aad5d2fd7814bad15)

## [Test containers](https://testcontainers.com/)
Test containers are used to allow easier integration testing. The test container setup is encapsulated inside the DatabaseFixture class that gets injected into each test class by the xUnit framework. NUnit is also supported.
There are no unit tests in this solution. The application seemed too simple to need any, but if there was more business logic in the app (e.g. if there were any services etc) then these would be expected to have unit tests created. 
