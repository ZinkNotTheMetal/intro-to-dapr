# Advanced Distributed Systems Design

- The terms and questions answered below are relevant for this class

## What is an application?

- An application has a single executable and runs on a single machine (i.e. Excel, Powerpoint, Work, etc.)
- Usually has a single source of information
- Don't typically 'know' about connectivity (network)

## What is a system?

- A system can be made up of multiple executable elements on multiple machines
- Usually has multiple sources of information
- Systems must deal with the concepts of networks and connectivity
- One of the problems of distributed systems is when we try to abstract away the network

## Common fallacies in enterprise computing

### 1. The network is reliable

- What happens when you get an HttpTimeoutException?
- Most developers will just log 'hey, something went wrong'
- Is this a long running process, did something happen on the way to the server?
- When do you retry?

#### Reliability Solutions

1. Retry & Ack / Store & Forward / Transactions
2. Use reliable messaging infrastructure
3. Don't roll your own solution

- Doesn't provide a request response synchronous method-centric model

HTTP gets most of the way there but it's not 100% we will need an additional technology to help with these problems & solutions

- Don't distribute your objects. Martin Fowler

### 2. Latency isn't a problem

- Given that we have serialized everything. How long does it take to get from the server to client
- Time to cross the network in one direction
- Small for LAN, WAN & Internet can be large
  - Many times slower than in-memory access
  - We need to understand the order of magnitude difference of making network calls and CPU cycles, we just typically understand that they are two lines of code
- Bad-old days of OO - remote objects (don't do remote objects!)
  - Even accessing a property was a round trip (not industry standard)
    - Now we use DTO's
  - But what about lazy-loading with an ORM?
    - Lazy loading is not immediate and we code with the assumption that the latency is 0, but it's not

#### Latency Solutions

1. Don't cross the network if you don't have to
2. Inter-object chit-chat shouldn't cross the network
3. If you have to cross hte network, take all the data you **might** need with you

### 3. Bandwidth isn't a problem

- Although bandwidth keeps growing, the amount of data grows faster
- When transferring lots of data in a given period of time, network congestion may interfere
- ORMs eagerly fetching too much data
- Subdivide the API / break up the monolith. Then we can put the higher priority items on a higher priority network

#### Bandwidth solutions

1. Move time-critical data to separate networks
   1. It's common to have more than one network in production
   2. We can also virtually subdivide the network
   3. High priority network & low priority network
2. Cnn't eagerly fetch everything / can't lazy load
3. have more than one domain model to resolve forces of bandwidth and latency

### 4. The network is secure

- Unless you're on a separate network that will never, ever be connected to anything else
- Well not even then. What about viruses, trojans, etc.
- Or what about users with USB, CDs, DVDs, w/e
- You can never be 100% safe from anything regarding the network
- Often it's the human vector that is the most dangerous one. Who would notice if yesterday's backup left the building?

#### Security solutions

1. Perform a threat model analysis
2. Balance cost against risk
3. Most importantly, talk about it include PR and legal

### 5. The network topology won't change

- Unless a server goes down and is replaced
- Or is moved to a different subnet
- Or connected to a VPN
- Or clients wireless connect and disconnect
  - Issues with WCF callback contracts (holding on to IP when communicating back and forth)
- What will happen to the system when those hard coded / config-file values change?

#### Topology solutions

1. (Obvious) Don't hardcode IP addresses
2. Consider using resilient protocols (multicast)
3. Discovery mechanisms are cool, but hard to get correct

- Will your system be able to maintain response-time requirements when it happens?

### 6. The admin will know what to do

- Possible in small networks
- Until they get ~~run over by a truck~~ promoted
  - Their replacement won't know what to do...
- If there are multiple admins, rolling out various upgrades and patches, will everything grind to a halt?
  - Will client software be able to work with a new version of the server?
- High availability while upgrading?
- Configurations can be spread everywhere (db, files, env files, etc.)
- Will "continuous deployment => "continuous unavailability"?

#### Admin solutions

1. Consider how to pinpoint problems in production
   1. Some logging is good, too much can be harmful
2. Consider multiple versions running in parallel
   1. Although backwards compatibility is hard
3. Enable the admin to take parts of the system down for maintenance without adversely affecting the rest
   1. Queuing helps

### 7. Transport cost isn't a problem

- Serialization before crossing the network (and deserialization on the other side) it takes time
- In the cloud, this can be a big cost factor
- The hardware network infrastructure has upfront and ongoing costs

#### Transportation cost solutions

1. The effect of serialization on performance further strengthen the argument to stay away from chatting over the network
2. Architects need to make trade-offs between infrastructure costs and development costs - upfront vs outgoing

### 8. The network is homogeneous

- It used to be easier - .NET/Java interop works
- Now we've got Ruby, NoSQL, and stuff people hacked together over http (aka. REST)
- Semantic interoperability will always be hard, budget for it

#### Technical solutions

1. Text on the wire (XML/JSON) - schema optional
2. Standards-based transfer protocol (http/smtp/udp)
3. Even better - host components in-process
4. Cross-compile to CLR/JVM

#### Semantic solutions

1. May require significant rewriting of data/rules
2. Try to identify semantic mismatches early

### 9. Tye system is atomic

- Maintenance is hard in "big balls of mud"
  - Changing one part of the system affects other parts (inadvertently)
- Integration through the DB creates coupling
  - It gets worse when we use XML (or JSON) columns in the DB
- If the system wasn't designed to scale out to multiple machines, doing so may hurt performance
  - Think of centralized databases or bottle neck systems

#### Atomic solutions

1. Internal loose coupling
2. Modularize
3. Design for scale out in advance, or you just may end up stuck with scale up!

### 10. The system is finished

- Maintenance costs over the lifetime of a system are greater than its development costs
- The system is never "finished"

#### Solution

1. There is no such thing as "maintenance"
2. Projects are a poor model for software development
   1. Long-lived projects are better (think iPhone)
3. Beware the rewrite will solve everything!

### 11. The business logic can and should be centralized

- Fancy way of saying reuse is good (DRY)
- Reusability begets high amounts of use begets high amount of dependencies which in turn creates tight coupling
- "First name must be less than 40 characters"
  - Okay, but where do we enforce it

- What about when the business rules change?

#### Centralized logic solution

1. Logic will be physically distributed
2. Can still 'centralize' in the development view
   1. 4+1 views of software architecture
3. Tag source code by feature implemented
4. Enables finding code by feature across multiple files

## Why messaging?

- Reduces afferent (incoming) and efferent (outgoing) coupling while increasing autonomy
- Reduces coupling
  - Use asynchronous messaging for temporal coupling
  - Use JSON/XML + **AMQP** for platform coupling

### Asynchronous messaging

- It's all about one-way, fire & forget messages
- Everything is built on top of it
  - Return Address pattern
  - Correlated Request/Response
  - Publish/Subscribe

### Performance - RPC vs Messaging

- With RPC, threads are allocated with load
- With messaging, threads are independent difference due to synchronous blocking calls
- Memory, DB locks, held longer with RPC
