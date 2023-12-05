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
