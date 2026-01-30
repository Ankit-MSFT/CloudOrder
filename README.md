# CloudOrder

CloudOrder is a sample cloud-native backend system built to explore **Azure architecture decisions** through a real, evolving project.

Instead of isolated tutorials, CloudOrder uses a single business scenario and evolves it step by step — covering backend APIs, storage, hosting, messaging, security, and observability on Azure.

This repository contains the **Part 1 (v1)** implementation: core APIs running locally using **.NET 10** and **Azure Table Storage**.

---

## Architecture Overview

CloudOrder v1 consists of three independent backend services:

- **Product API** – product catalog and stock management  
- **Customer API** – customer profile and address management  
- **Order API** – order creation and order status tracking  

Each service:
- owns its own data
- exposes a small REST API
- persists data using Azure Table Storage
- is designed to evolve independently

---

## Tech Stack

- .NET 10  
- ASP.NET Core Web API  
- Azure Table Storage (`Azure.Data.Tables`)  
- Azurite (local development)  
- Azure Storage Explorer  

---

## Solution Structure

```text
CloudOrder/
├── CloudOrder.Contracts          # Shared DTOs and enums
├── CloudOrder.Product.Api        # Product microservice
├── CloudOrder.Customer.Api       # Customer microservice
├── CloudOrder.Order.Api          # Order microservice
└── CloudOrder.slnx
````

---

## Running Locally

### Prerequisites

* .NET 10 SDK
* Azurite
* Azure Storage Explorer
* Postman or curl

---

### Start Azurite

```bash
azurite
```

Azurite runs Azure Table Storage locally on default ports.

---

### Run Product API

```bash
cd CloudOrder.Product.Api
dotnet run
```

The API will start on a local port (check console output).

---

### Test the API

Use Postman or curl to test:

* `POST /api/products`
* `GET /api/products`
* `GET /api/products/{productId}`
* `PATCH /api/products/{productId}/stock`

You can verify stored data using **Azure Storage Explorer** connected to Azurite.

---

## Storage Design

Azure Table Storage modeling:

| Table     | PartitionKey | RowKey     |
| --------- | ------------ | ---------- |
| Products  | PRODUCT      | productId  |
| Customers | CUSTOMER     | customerId |
| Orders    | customerId   | orderId    |

This design optimizes common query patterns and keeps storage simple for v1.

---

## Design Principles

* DTOs define API contracts
* Domain models represent business concepts
* Storage entities are isolated behind repositories
* Controllers depend on repository interfaces
* Storage can be swapped (Table → Cosmos DB) without changing APIs

---

## Project Status

* ✔ Product, Customer & Order APIs implemented and tested locally using Azurite
* ⬜ Azure hosting comparisons
* ⬜ Messaging, security, observability

---

## Related Blog Series

This repository accompanies a blog series that documents the architecture and design decisions behind CloudOrder.

* Part 1A – [Architecture and domain design](https://www.azuretechinsights.com/2026/01/designing-microservices-hands-on.html)
* Part 1B – Building APIs locally with Azure Table Storage

(Blog links will be added as the series progresses.)

---

## Disclaimer

CloudOrder is an educational project designed to demonstrate architectural concepts.
It is not intended for production use.

---

## License
MIT


