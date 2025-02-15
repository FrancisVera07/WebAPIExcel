using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebAPIExcel.Models;

public class ExcelContext : DbContext
{
    public ExcelContext(DbContextOptions<ExcelContext> options) : base(options) {}
    public DbSet<ExcelItem>? ExcelItems { get; set; } = null;
}