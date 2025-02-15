using Microsoft.EntityFrameworkCore;

namespace WebAPIExcel.Models;

public class UnitContext : DbContext
{
    public UnitContext(DbContextOptions<UnitContext> options) : base(options) {}
    public DbSet<UnitItem>? UnitItems { get; set; }
}