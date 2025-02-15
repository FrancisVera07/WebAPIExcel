using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models;

public class UnitItem
{
    public long Id { get; private set; }
    [Required]
    public String? Name { get; set; }
    public DateTime? CreateTimestamp { get; private set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; private set; } = DateTime.Now;
}