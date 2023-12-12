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
