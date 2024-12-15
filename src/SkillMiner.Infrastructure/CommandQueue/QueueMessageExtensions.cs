using Microsoft.EntityFrameworkCore;
using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Infrastructure.CommandQueue;

public static class CommandQueueMessageExtensions
{
    public static void AddCommandMessageQueue(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommandQueueMessage>(entity =>
        {
            // Configures the table for storing command queue messages.
            entity.ToTable("CommandQueueMessages");
        });
    }
}
