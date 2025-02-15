using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models;

public class ConceptItem
{
    public long Id { get; private set; }
    [Required]
    public String? Key { get; set; }
    [Required]
    public String? Concept { get; set; }
    [Range(0, double.MaxValue)]
    public float? Quantity { get; set; }
    [Range(0, double.MaxValue)]
    public float? UnitPrice { get; set; }
    [Range(0, double.MaxValue)]
    public float? Amount { get; set; }
    
    // Relación con GroupParentItem y UnitItem
    public long? GroupParentID { get; set; }
    public long? UnitID { get; set; }
    
    public GroupParentItem? GroupParentItem { get; set; }
    public UnitItem? UnitItem { get; set; }
    
    public DateTime? CreateTimestamp { get; private set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; private set; } = DateTime.Now;
}