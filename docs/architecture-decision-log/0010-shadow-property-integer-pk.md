# 9. Shadow Property Integer PK

## Status

Accepted

## Context

GUIDs are often used as primary keys in databases due to their global uniqueness and ease of use for distributed systems. However, when GUIDs are used as clustered primary keys in SQL Server, they can cause performance issues, particularly page fragmentation. This fragmentation arises because GUIDs are not sequential, leading to inefficient allocation of storage and slower read/write operations.

To address these performance concerns while preserving the use of GUIDs for their unique identification capabilities, we propose the introduction of an integer-based primary key as a shadow property in Entity Framework Core.

## Decision

We will implement the following design:

- **Shadow Property for Integer Primary Key**:
Each entity will have an integer property called DatabaseId, configured as a shadow property in Entity Framework Core. This property will serve as the primary key and the clustered index in the database.

- **GUID Identifier for Domain Access**:
Each entity will also have a GUID property called Id. This property will be used exclusively in the application code to uniquely identify entities. It will have a non-clustered index in the database to support efficient querying.

- **Code Perspective**:
The integer primary key (DatabaseId) will not be exposed in the application code. From the perspective of the code, the GUID Id is the only identifier available for entities.

### Entity Configuration Example

```csharp
internal sealed class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        builder.ToTable(nameof(JobListing));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new JobListingId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(JobListing)}_Id")
            .IsUnique();
        #endregion
    }
}
```

## Consequences

### Advantages

- **Improved Database Performance** - By using an integer as the clustered primary key, we reduce page fragmentation and improve the performance of read and write operations in SQL Server.

- **Separation of Concerns** - The shadow property allows the database infrastructure to optimize for performance without affecting the application code.

- **Preservation of GUID Benefits** - The GUID Id remains globally unique and serves as the primary identifier in the application.

### Disadvantages

- **Increased Complexity** - The dual identifier approach introduces additional configuration and potential for confusion during debugging or database inspection.
