# Architectural Decision Document for ASP.NET Core 8 API

## Overview

This document outlines the architectural decisions for the backend ASP.NET Core 8 API project. The architecture is
designed with a focus on strong domain modeling, clear separation of concerns, robust error handling, and modern best
practices.

## 1. Project Structure

The project is organized into distinct layers to ensure a clear separation of concerns and maintainability:

- **Domain Layer:** Contains all domain logic modeled using DDD tactical patterns.
- **Application Layer:** Contains all HTTP endpoints, handling the interaction between the client and the domain layer.

### Domain Layer

The domain layer is responsible for encapsulating the core business logic of the application. It uses the following DDD
tactical patterns:

- **Aggregate Roots, Entities, and Value Objects:** These patterns help in modeling the domain with a clear boundary and
  encapsulation of business logic.
- **Strongly Typed IDs:** Used to enforce type safety and clarity in the domain model.
- **Domain Events:** Propagate modifications between entities, handled by event handlers using MediatR.

### Application Layer

The application layer handles the interaction with clients through HTTP endpoints:

- **FastEndpoints:** Used for defining endpoints, ensuring each endpoint is responsible for a single business
  transaction.
- **Request and Response DTOs:** Ensure that the domain model is not exposed directly to the clients.
- **Request Validation:** All endpoints are responsible for validating the incoming requests directly.

## 2. Error Handling

### Domain Errors

All errors in the domain are represented with custom exceptions containing an error code and an error type, defined as
follows:

```csharp
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
}
```

When an exception occurs during business transaction handling, it will stop the current transaction.

### API Error Handling

- **Problem Details Standard:** All errors returned by endpoints follow the Problem Details standard for consistency.
- **Middleware for Error Translation:** A middleware component catches all thrown errors and translates domain errors to
  appropriate HTTP status responses.

## 3. Event Handling

- **MediatR:** Used for handling domain events, ensuring that changes within the domain propagate correctly and in a
  decoupled manner.
- **Integration Events:** For logic that needs to be outside the scope of an endpoint, integration events are stored in
  an outbox table as part of the same transaction as the business logic. These events are published by a job and handled
  by idempotent integration handlers.
- **Hangfire:** Manages the job logic for publishing integration events.

## 4. Persistence

- **Entity Framework:** Used as the ORM with a single DbContext for all application logic. This ensures a consistent and
  centralized approach to database interactions.

## 5. Testing

- **Integration Tests:** All endpoints are tested with integration tests to ensure the entire application stack works
  correctly. Integration testing uses Testcontainers to instantiate the database and other required services.
- **Unit Tests:** All domain logic is thoroughly tested with unit tests to ensure correctness and reliability of the
  core business logic.
- **Test Data Factories:** All data needed for tests is created in separate factories and reused in tests that need
  them. These data factories are customizable to allow easy arrangement of test data and are stored in a separate common
  project for test purposes only.

## 6. User Authentication and Security

- **Keycloak:** Used for user authentication, federating multiple authentication modes.
- **JWT Tokens:** For security, JWT tokens issued by Keycloak are used.

## 7. Deployment and Local Development

- **Docker:** Used locally for setting up the project.
- **Health Checks:** Defining health liveness and readiness endpoints:
    - **Liveness Endpoint:** Indicates that the site is up.
    - **Readiness Endpoint:** Indicates if the site is fully operational, checking all dependencies. If a dependency is
      required for performance but not critical, it changes the status to deprecated rather than unhealthy.

## 8. Shared Kernel

A shared kernel will be created to hold all common objects and logic that is transversal to all projects, excluding the
specific domain logic of this project.

## 9. Observability

- **OpenTelemetry:** Used for observability, exposing logs, traces, and metrics.

## 10. Localization

Only content produced by the backend (e.g., documents, emails) will be localized. Other content is not localized.

## 11. API Documentation

- **Swagger UI:** Used for API documentation. All endpoints must have correct descriptions containing a summary,
  detailed description, responses, and request types with examples at minimum.

## Conclusion

This architecture ensures a robust, maintainable, and scalable backend for the ASP.NET Core 8 API. By leveraging DDD
patterns, MediatR for event handling, strong error handling practices, and modern tools like FastEndpoints, Entity
Framework, Hangfire, and OpenTelemetry, the architecture provides a solid foundation for building complex business
applications. The use of Docker, Keycloak, and Testcontainers further enhances the development, testing, and deployment
processes, ensuring high performance and reliability.