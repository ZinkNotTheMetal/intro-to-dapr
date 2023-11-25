# Getting Started

## What is dapr?

- [dapr.io](https://dapr.io)

### Prerequisites

- Docker
The recommended development environment requires Docker. While you can initialize Dapr without a dependency on Docker

- You can also install Podman in place of Docker. Read more about initializing Dapr using Podman.

## Resources

- [Getting started with dapr](https://docs.dapr.io/getting-started/)

## Creating a project

### Steps

#### Install the dapr cli

```sh
brew install dapr/tap/dapr-cli
```

Verify the installation

```sh
dapr -h
```

#### Install the runtime binaries

```sh
dapr init
```
