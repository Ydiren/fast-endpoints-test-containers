# FastEndpoints and TestContainers
## Description

A small project to demonstrate the usage of both FastEndpoints and Test Containers in the same solution. It uses Serilog to log to both the console and a local Seq server when run in the local dev environment.
The local environment can be configured using the docker-compose.yml file in the root of the solution. This will spin up a local Seq server and a local Postgres Server instance. The Seq server is used to view structured logs from the application.


This is about as bare bones as a project can get. There's plenty of scope to expand this (e.g. implement middleware to add correlation ids, logging etc) but it should get the overall gist of the architectural pattern.

## [FastEndpoints](https://fast-endpoints.com/)<br/>
FastEndpoints uses the REPR pattern (Request-EndPoint-Response) to keep all classes/files related to the endpoint in close proximity (usually all within the same file)

Out of the box, FastEndpoints provides patterns for easily implementing Endpoints, validators and mappers. Using the provided snippets it becomes really easy to generate a new endpoint, all of the boiler plate is done for you!
#### FastEndpoint Resources
[Snippets documentation](https://fast-endpoints.com/docs/scaffolding#feature-scaffolding)<br/>
[Visual Studio Snippets](https://marketplace.visualstudio.com/items?itemName=dj-nitehawk.FastEndpoints)<br/>
[Visual Studio Code Extension](https://marketplace.visualstudio.com/items?itemName=drilko.fastendpoints)<br/>
[Rider Snippets](https://gist.github.com/dj-nitehawk/6493cb85bf3bb20aad5d2fd7814bad15)

## [Test containers](https://testcontainers.com/)
Test containers are used to allow easier integration testing. The test container setup is encapsulated inside the DatabaseFixture class that gets injected into each test class by the xUnit framework. NUnit is also supported.
There are no unit tests in this solution. The application seemed too simple to need any, but if there was more business logic in the app (e.g. if there were any services etc) then these would be expected to have unit tests created. 

## [Serilog](https://serilog.net/) structured logging
This package provides an abstraction over the logging mechanisms used in the application. It is configured to log to both the console and to the local Seq instance when run in the local dev environment. When run in the test environment, it will only log to the console. This is configured in the appsettings.Development.json file.

To log structured data, use the `Log.Information("Message {Property1} {Property2}", property1, property2)` syntax **(*Note the lack of a leading `'$'` before the opening quote of the message!*)**.
The properties will be logged as key value pairs in the structured log. The properties can be accessed in Seq by using the `@Property1` syntax.


## [Seq](https://datalust.co/seq) structured log server
The logs can be access by navigating to http://localhost:8081 in the browser. The default view shows one line per log entry, but clicking on a log entry will expand that log and display the full log information. The logs can be filtered by clicking on the filter icon in the top right of the screen. The filter syntax is documented [here](https://docs.datalust.co/docs/query-language).