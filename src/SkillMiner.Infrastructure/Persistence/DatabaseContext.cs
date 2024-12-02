using Microsoft.EntityFrameworkCore;

namespace SkillMiner.Infrastructure.Persistence;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options);
