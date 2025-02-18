using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models.DTOs;

public class ConceptItemDTO
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
}