namespace WebAPIExcel.Models;

public class GroupParentItem
{
    public long id { get; private set; }
    
    // Relacion con GroupItem y ParentItem
    public long? GroupID { get; set; }
    public long? ParentID { get; set; }
    
    public GroupItem? GroupItem { get; set; }
    public GroupItem? ParentItem { get; set; }
    
    public DateTime? CreateTimestamp { get; private set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; private set; } = DateTime.Now;
}