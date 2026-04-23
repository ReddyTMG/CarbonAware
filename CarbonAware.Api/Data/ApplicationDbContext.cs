using Microsoft.EntityFrameworkCore;
using CarbonAware.Api.Models;

namespace CarbonAware.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    // This property tells EF Core to create a table named "CarbonLogs"
    public DbSet<CarbonLog> CarbonLogs => Set<CarbonLog>();
}