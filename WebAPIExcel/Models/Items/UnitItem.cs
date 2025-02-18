using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIExcel.Models.Items;

public class UnitItem
{
    public long? Id { get; set; }
    [Required]
    public String? Name { get; set; }
    
    // Relacionar con ConceptItem
    [JsonIgnore]
    public ICollection<ConceptItem> ConceptItems { get; set; } = new List<ConceptItem>();
    
    public DateTime? CreateTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; set; } = DateTime.Now;
}