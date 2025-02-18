using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIExcel.Models.Items;

public class ConceptItem
{
    public long Id { get; set; }
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
    [Required]
    public long? GroupParentID { get; set; }
    [Required]
    public long? UnitID { get; set; }
    
    [JsonIgnore]
    public GroupParentItem? GroupParentItem { get; set; }
    [JsonIgnore]
    public UnitItem? UnitItem { get; set; }
    
    public DateTime? CreateTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; set; } = DateTime.Now;
}