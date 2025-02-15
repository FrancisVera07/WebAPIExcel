using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models;

public class GroupItem
{
    public long Id { get; private set; }
    [Required]
    public String? Key { get; set; }
    [Required]
    public String? Concept { get; set; }
    
    // Relación con ExcelItem
    public long? ExcelFileID { get; set; }
    public ExcelItem? ExcelFile { get; set; }
    
    public DateTime? CreateTimestamp { get; private set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; private set; } = DateTime.Now;
}