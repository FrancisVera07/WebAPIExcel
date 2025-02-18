using Microsoft.EntityFrameworkCore;
using WebAPIExcel.Models.Items;

namespace WebAPIExcel.Models.Contexts;

public class AppDbContext : DbContext
{
    // Contexto de base de datos
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    // Items a utlilizar
    public DbSet<ExcelItem> ExcelItems { get; set; } = null;
    public DbSet<GroupItem> GroupItems { get; set; } = null;
    public DbSet<GroupParentItem> GroupParentItems { get; set; } = null;
    public DbSet<UnitItem> UnitItems { get; set; } = null;
    public DbSet<ConceptItem> ConceptItems { get; set; } = default!;
    
    // Relaciones entre modelos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación Group con ExcelFile
        modelBuilder.Entity<GroupItem>()
            .HasOne(g => g.ExcelFile)
            .WithMany(e => e.GroupItems)
            .HasForeignKey(g => g.ExcelFileID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relación GroupParent con Group
        // Relación GroupParent Padre
        modelBuilder.Entity<GroupParentItem>()
            .HasOne(gp => gp.ParentItem)
            .WithMany(g => g.ParentGroupItems)
            .HasForeignKey(gp => gp.ParentID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relación GroupParent Hijo
        modelBuilder.Entity<GroupParentItem>()
            .HasOne(gp => gp.GroupItem)
            .WithMany(g => g.ChildGroupItems)
            .HasForeignKey(gp => gp.GroupID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relación Concept con Unit
        modelBuilder.Entity<ConceptItem>()
            .HasOne(c => c.UnitItem)
            .WithMany(u => u.ConceptItems)
            .HasForeignKey(c => c.UnitID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relación Concept con GroupParent
        modelBuilder.Entity<ConceptItem>()
            .HasOne(c => c.GroupParentItem)
            .WithMany(gp => gp.ConceptItems)
            .HasForeignKey(c => c.GroupParentID)
            .OnDelete(DeleteBehavior.Cascade);
        
    }

// public DbSet<WebAPIExcel.Models.Items.UnitItem> UnitItem { get; set; } = default!;
}