# Useful patterns with distributed systems

## SOA

- Service-oriented architecture (SOA) is a way of designing software applications using reusable and interoperable components called services.
  - Services are in the logical view
  - Mapping to the development view, a service could be a source control repository itself
  - Publishing schema to a package manager (NuGet) will help us decouple services

### What is a service?

- A service is the technical authority for a specific business capability (all data & business rules related to that business capability belong to that service)
- All data and business rules reside within the service
- Nothing is 'left over' after identifying services
- Everything must be in 'some' service

### What a service is not

- A service that has only functionality is a __function__ not a service.
  - i.e. - calculation or validation
- A service that has only data is a __database__ not a service
  - i.e. CRUD entities
- WSDL / REST doesn't change logical responsibility

### Tenant's of a service

1. Services are autonomous
2. Services have explicit boundaries
3. Services share contract & schema, not class or type (or anything else including databases!)
4. Service interaction is controlled by policy (physical communication on how services communicate)

## Event sourcing

- Instead of storing just the current state of the data in a domain, use an append-only store to record the full series of actions taken on that data. The store acts as the system of record and can be used to materialize the domain objects. This can simplify tasks in complex domains, by avoiding the need to synchronize the data model and the business domain, while improving performance, scalability, and responsiveness. It can also provide consistency for transactional data, and maintain full audit trails and history that can enable compensating actions.

## Saga pattern

- A failure management pattern that helps establish consistency in distributed applications and coordinates transactions between multiple microservices to maintain data consistency

## Task Chaining pattern

- Series of activities that happen in sequential order. The output of one step may be passed as the input to the next step.
- The steps of the workflow may need to be orchestrated across multiple microservices.
- Need to implement retry policies and compensating transactions in case calls to a particular service fail

## Monitor Pattern

- The Monitor Object pattern is a concurrency design pattern that synchronizes method execution in multi-threaded applications by encapsulating synchronization within an object. This pattern is particularly useful for managing access to shared resources and ensuring that only one thread at a time can access or modify the shared resource. Monitor Objects help simplify synchronization and improve thread safety by encapsulating the synchronization mechanism and providing a clear interface for clients.

### Monitor pattern uses

1. You want to manage access to shared resources in a multi-threaded environment.
2. You need to ensure that only one thread at a time can access or modify the shared resource.
3. You want to encapsulate synchronization mechanisms and provide a clear interface for clients.

## Fan in / Fan out pattern

- Fan in / Fan out pattern - consists of executing multiple functions concurrently and then performing some aggregation on the results.
- This pattern essentially means running multiple instances of the activity function at the same time. The “fan out” part is the splitting up of the data into multiple chunks and then calling the activity function multiple times, passing in these chunks. The fanning out process invokes multiple instances of the activity function. When each chunk has been processed, the “fan in” takes places and takes the results from each activity function instance and aggregates them into a single final result.

## UI composition

- Compose a user interface from multiple different parts that can be managed and deployed separately.
- With UI composition, what appears to be a single monolithic user interface is in fact made up of multiple separate components. These components can be served from different backing systems (perhaps separate microservices), and can be managed by different teams.
- These components can be changed independently from each other, allowing different teams to work in parallel and push out changes as and when they are ready.

## Messaging Patterns

### Return Address Pattern

- When sending a message if you expect a response, you should include an address on where you are listening for that response
- Similar to a postage note (writing a return address on a letter)
- Simple header that needs to be set

### Publish / Subscribe

- In software architecture, publish–subscribe is a messaging pattern where publishers categorize messages into classes that are received by subscribers. This is contrasted to the typical messaging pattern model where publishers send messages directly to subscribers.
- Asynchronous messaging is an effective way to decouple senders from consumers, and avoid blocking the sender to wait for a response. However, using a dedicated message queue for each consumer does not effectively scale to many consumers.

### Claim Check Pattern

- Split a large message into a claim check and a payload. Send the claim check to the messaging platform and store the payload to an external service. This pattern allows large messages to be processed, while protecting the message bus and the client from being overwhelmed or slowed down. This pattern also helps to reduce costs, as storage is usually cheaper than resource units used by the messaging platform.

## dapr workflows

- Dapr workflows are stateful orchestration mechanism that helps in building custom and long running business logic in a reliable manner. As it is stateful, it can be used to reactive the workflow incase of disruptions picking up where it left off. It helps in building reliable orchestration across multiple microservices

- Activity (i.e. - Step) - are basic units of work that are being done in the workflow. These are the tasks that get executed in a processing order to produce a result. Dapr workflow engine guarantees that each workflow is at least executed once. That's why it is required to make sure activity logic should be idempotent.

### Benefits of workflows in dapr

1. Takes care of executing the workflow and ensuring that it runs to completion
2. Saves progress automatically
3. Automatically resumes the workflow from the last completed step if the workflow process itself fails for any reason
4. Provides built-in retry configuration primitives to simplify the process of configuring complex retry policies for individual steps in the workflow
5. Supports external system interaction in case you need to wait for a user / process to approve before moving on to the next step