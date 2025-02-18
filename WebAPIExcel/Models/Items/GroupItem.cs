using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIExcel.Models.Items;

public class GroupItem
{
    public long Id { get; set; }

    [Required]
    public string Key { get; set; }

    [Required]
    public string Concept { get; set; }

    // Relación con ExcelItem
    [Required]
    public long ExcelFileID { get; set; }

    [JsonIgnore]
    public ExcelItem? ExcelFile { get; set; }

    // Relación GroupParentItems con GroupItems
    [JsonIgnore]
    public ICollection<GroupParentItem> ParentGroupItems { get; set; } = new List<GroupParentItem>();
    [JsonIgnore]
    public ICollection<GroupParentItem> ChildGroupItems { get; set; } = new List<GroupParentItem>();

    public DateTime? CreateTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; set; } = DateTime.Now;
}