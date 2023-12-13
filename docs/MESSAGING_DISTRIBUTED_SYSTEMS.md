# Why messaging?

- Reduces afferent (incoming) and efferent (outgoing) coupling while increasing autonomy
- Reduces coupling
  - Use asynchronous messaging for temporal coupling
  - Use JSON/XML + **AMQP** for platform coupling
- We want to avoid having Service A and Service B having a shared database that both are talking to

## Asynchronous messaging

- It's all about one-way, fire & forget messages
- Everything is built on top of it
  - Return Address pattern
  - Correlated Request/Response
  - Publish/Subscribe

## Performance - RPC vs Messaging

- With RPC, threads are allocated with load
- With messaging, threads are independent difference due to synchronous blocking calls
- Memory, DB locks, held longer with RPC
- In a single call to a single RPC it tends to perform better than messaging... but, at scale messaging tends to be more reliable and scalable compared to RPC

## Service interfaces vs strongly typed messages

- Problem is that service layers get too large
- Difficult to scale a large single interface
- Difficult for multiple developers to collaborate (git issues)
- Difficult to reuse logging, auth, etc.

## Messaging fault tolerance

- Servers go down
- Databases go down
- Deadlocks occur in the database

## Auditing / Journaling

- Sends a copy of the message to another queue when it is processed
  - Supported out-of-the-box by most queues
  - Extract to longer-term storage (so the queue doesn't explode with messages)
- A central log of everything that happened
- Can be difficult to interpret by itself
  - Helpful if we add breadcrumbs to the messages by adding headers when a message is processed
- Adding IDs & headers to messages help us understand more what is happening in the system

## Bus & Broker

- An architectural style is a coordinated set of architectural constraints that restricts the roles/features of architectural elements and the allowed relations among those elements within any architecture that conforms to that style
  - Fielding (2000)
  - What is / isn't allowed in an architecture
  - Not meant to be complete binding rules
  - Doesn't say there can only be one
  - Multiple styles in a project
    - Layering, MVC, pipes & filters, etc.
- You might need both within the same project

- Both architectural styles (bus & broker) attempt to reduce spatial coupling (different approaches)
- Very little in common between them

### Broker

- Also known as "Hub and Spoke" and "mediator" designed to avoid having to change apps - EAI (Enterprise Application Integration)
- Broker is physically separate
- ALL communication goes through the broker
- Broker handles fail over, routing
- Broker is a single point of failure, must be robust and performant

#### Broker advantages

- Concentrating all communications to a single logical entity, enables central management
- Enables "intelligent" routing, data transformation, orchestration
- Doesn't require changes to surrounding apps

#### Broker disadvantages

- Embodies the 11th fallacy:
  - Business logic can and should be centralized
- Procedural programming at a large scale without good unit testing or source control
- Prevents apps from gaining autonomy

### Bus

- Predated the broker architecture
- Event source and sinks use bus for pub/sub
- Designed to allow independent evolution of sources and sinks
- Bus is not necessarily physically separate
- Communication is distributed no single point of failure
- Bus is simpler - no content-based routing or data transformations
- Orthogonal to the broker style

#### Bus advantages

- No single point of failure
- Doesn't break service autonomy

#### Bus disadvantages

- More difficult to design distributed solutions than centralist ones