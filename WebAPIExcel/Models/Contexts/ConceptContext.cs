using Microsoft.EntityFrameworkCore;

namespace WebAPIExcel.Models;

public class ConceptContext : DbContext
{
    public ConceptContext(DbContextOptions<ConceptContext> options) : base(options) {}
    public DbSet<ConceptItem> ConceptItems { get; set; }
    // Referencia a Unit y GroupParent
    public DbSet<GroupParentItem> GroupParentItems { get; set; }
    public DbSet<UnitItem> UnitItems { get; set; }

    /**
     * Relación entre GroupParentItems y UnitItems
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConceptItem>()
            .HasOne(c => c.GroupParentItem)
            .WithMany()
            .HasForeignKey(c => c.GroupParentID)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ConceptItem>()
            .HasOne(c => c.UnitItem)
            .WithMany()
            .HasForeignKey(c => c.UnitID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
