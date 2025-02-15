using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace WebAPIExcel.Models;

public class GroupContext : DbContext
{
    public GroupContext(DbContextOptions<GroupContext> options) : base (options) {}
    public DbSet<GroupItem>? GroupItems { get; set; }
    // Referencia al contexto de Excel
    public DbSet<ExcelItem>? ExcelItems { get; set; }

    /**
     * Relación entre GroupItem y ExcelItem
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupItem>()
            .HasOne(g => g.ExcelFile)
            .WithMany()
            .HasForeignKey(g => g.ExcelFileID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}