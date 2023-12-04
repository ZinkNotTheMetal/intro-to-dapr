# Coupling in Distributed Systems

## What is coupling?
- A measure of dependencies
- If X depens on Y, there is 'coupling' between them
- 2 different types of coupling
  - Afferent (Ca)
  - Efferent (Ce)
- In the scenario above
  - X is efferently coupled to Y
  - Y is afferently coupled to X

- As the __efferent__ coupling increases it is difficult to believe that the class follows the Single Responsibility principal
  - An example, if a class has 50 interfaces. How could it implement all 50 and not validate the single responsibilty
- Afferent coupling is typically with a generic low level library (i.e. Logging, ORM, etc.)
- Efferent coupling is typically related to domain specific calls and this is the more dangerous coupling


### What is afferent coupling
- Who depends on you (incoming coupling)
- Examples (DTOs, Logging)

### What is efferent coupling
- Who do you depend on (outgoing coupling)
- Examples (Calling business calculations / processes, presentation layer)

## How to count coupling?

X -> Y
 - 1 method call

A -> B
 - 3 method calls
 - 2 properties

- The incoming coupling in B & Y is the same (one)
- The outgoing coupling in A is 5

### What number is ok?

- A number doesn't matter as much as the excercise
  - Identify coupling
  - Use tooling that counts coupling correctly
  - Measure and discuss periodically
- Ensure to keep numbers way down when a class has afferent and efferent coupling in the same class. This is typically a receipe for disaster
- C# - NDepend / XDepend
- Use Typescript static analysis

## Becareful of Shared Resources

- They hide the coupling that otherwise would be visible

## Loose Coupling at the systems level

- Minimize afferent and efferent coupling
  - But not mechanically
- Zero coupling isn't really possible
- 3 different aspects of coupling for systems:
    1. Platform
    2. Temporal
    3. Spatial

### Platform

- Also known as 'interoperability'
- Using protocols only available on one platform
  - Remoting, binary serlization, priority choices, etc.
- One of the 4 tenents of Service Orientation
  - Share contract and schema, not class or type

#### Platform solutions

1. Text based representation on the wire (XML/JSON)
2. Use standard protocols like http / smtp / udp
3. SOAP (provides a definition of an envelope - REST doesn't without more work) / WSDL (given a request structure / what will be the response structure) / REST
4. Running Java code in-process on the CLR
5. Running C# code in-process on the JVM
  - JNBridge
  - No network reliability
  - No bandwidth concerns
  - No security conerns

### Temporal coupling

- The coupling on the dimension of time
- Service A calls Service B - using a synchronous call
- A has a high degree of temporal coupling on B

#### Temporal solutions

1. DO NOT write multi-threaded code. and 'becareful' of async / await 
2. It's not a question of if you're going to cache it's only a matter of where
3. In a distributed system you __will__ have stale data!
  1. The only way to prevent this is to open a distributed transaction through all services and DB - doesn't scale... don't do this

#### Pub/sub temporal constraints

1. Subscriber must be able to make decisions based on somewhat stale data (need to verify this with the business)
2. Requires a strong division of responsibility between publishers and subscribers
3. Only one logical publisher should be able to publish a given kind of event

#### How to design events

1. __Avoid__ requests/commands
  - Bad example: 'SaveCustomerRequested'
2. Save something that happened (past tense)
  - Subscribers shouldn't be able to invalidate this
  - Good example: 'OrderAccepted'
3. If you have to talk about the data, state its validity
  - ProductPriceUpdated { Price: $5, ValidTo: 1/1/15 }
  - Helpful if the event comes in out of order

#### Message type = logical destination

- AddCustomerMessage:
  Sent by clients to one logical server
  Multiple physical servers behind a load balancer
- OrderCancelledEventMessage:
  Published by one logical server
  Multiple physical servers can publish the same message
- Strongly-typed messages simplify routing vs document-centric messaging

### Where (and why) not to do Pub/sub

1. When business requirements demand consistency! (fancy way of business telling you your architecture is wrong)
2. It's hardwork to find the boundries of where pub/sub makes sense

### Spatial

- How bound am I to a specific machine in order for my execution to operate
- IP hardcoding / no load balancing
- Mainly a deployment concern but can be a degree of coupling not always considered

####  Spatial solutions

1. Application level code should not need to know where the cooperating services are on the network
2. Delegate communications to lower layer - service agent pattern
3. Clients talking to servers through a load balancer don't know which physical server is handling the request...
4. Routing is first logical, and second (after) a physical concern