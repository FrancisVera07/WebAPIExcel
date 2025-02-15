using Microsoft.EntityFrameworkCore;

namespace WebAPIExcel.Models;

public class GroupParentContext : DbContext
{
    public GroupParentContext(DbContextOptions<GroupParentContext> options) : base (options) {}
    DbSet<GroupParentItem>? GroupParentItems { get; set; }
    // Referencia al contexto de Group
    public DbSet<GroupItem>? GroupItems { get; set; }

    /**
     * Relación entre GroupParentItems y GroupItem
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación para GroupID
        modelBuilder.Entity<GroupParentItem>()
            .HasOne(gp => gp.GroupItem)
            .WithMany()
            .HasForeignKey(gp => gp.GroupID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relación para ParentID
        modelBuilder.Entity<GroupParentItem>()
            .HasOne(gp => gp.ParentItem)
            .WithMany()
            .HasForeignKey(gp => gp.ParentID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}