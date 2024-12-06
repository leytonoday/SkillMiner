# 6. Unit Of Work Pattern

## Status

Accepted

## Context

In the development of SkillMiner, effective management of database transactions is crucial for maintaining data integrity and streamlining development.

## Decision

We will implement the Unit of Work pattern. This pattern centralizes transaction management, ensuring atomic, consistent, isolated, and durable (ACID) operations.

It will serve to abstract away the interaction with any particular DB technology for transactions.

The interface will exist in the Domain layer, as recommended in the following article: <https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core>

> "It is also important to mention that the IUnitOfWork interface is part of your domain layer, not an EF Core type.""

## Consequences

- **Data Consistency** - Ensures changes are either committed together or rolled back together.

- **Transaction Management** - Simplifies transaction handling across multiple repositories.

- **Learning Curve** - Requires understanding of the pattern's concepts.
