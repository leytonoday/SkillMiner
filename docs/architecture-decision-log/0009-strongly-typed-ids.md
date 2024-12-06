# 9. Using Strongly Typed IDs

## Status

Accepted

## Context

Traditionally, systems have used primitive types (e.g., int, string, or Guid) for entity identifiers. While this approach is simple and widely understood, it has notable drawbacks:

- **Lack of Type Safety** - It is easy to accidentally assign or compare identifiers of different entities (e.g., assigning a CustomerId to a ProductId).

- **Reduced Code Readability** - Primitive types provide no context about what an identifier represents.

- **Increased Risk of Bugs** - Using primitive types can lead to subtle bugs when identifiers are misused or confused.

In complex systems, these issues become more pronounced, especially when multiple entities use the same identifier type (e.g., int or Guid). Strongly typed IDs provide a solution by wrapping primitive types in specific value objects, offering better type safety, clarity, and maintainability.

## Decision

We will use strongly typed IDs for entity identifiers instead of primitive types. Each entity will have its own ID type, implemented as a value object that wraps a primitive type. For example:

```csharp
public abstract record EntityId(Guid Value)
{
  public override string ToString() => Value.ToString();
}

public record JobListingId(Guid Value) : EntityId(Value);
```

## Consequences

### Advantages

- **Improved Type Safety** - It becomes impossible to accidentally mix up identifiers for different entities, reducing bugs.

- **Better Code Readability** - Strongly typed IDs convey their purpose and context explicitly, improving clarity for developers.

- **Alignment with DDD Principles** - Strongly typed IDs align with Domain-Driven Design principles by treating identifiers as domain value objects.

- **Easier Refactoring** - Changes to the identifier implementation (e.g., switching from int to Guid) are localized to the strongly typed ID class, simplifying maintenance

### Disadvantages

- **Increased Boilerplate** - Creating a strongly typed ID class for every entity introduces additional code.

- **Serialization Complexity** - Care must be taken to handle serialization and deserialization of strongly typed IDs when interacting with external systems or databases.
