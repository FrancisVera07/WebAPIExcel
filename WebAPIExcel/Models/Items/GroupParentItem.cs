using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIExcel.Models.Items;

public class GroupParentItem
{
    public long Id { get; set; }
    
    // Relacion con GroupItem y ParentItem
    [Required]
    public long GroupID { get; set; }
    [Required]
    public long ParentID { get; set; } 
    
    [JsonIgnore]
    public GroupItem? GroupItem { get; set; }
    [JsonIgnore]
    public GroupItem? ParentItem { get; set; }
    
    [JsonIgnore]
    public ICollection<ConceptItem> ConceptItems { get; set; } = new List<ConceptItem>();
    
    public DateTime? CreateTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; set; } = DateTime.Now;
}