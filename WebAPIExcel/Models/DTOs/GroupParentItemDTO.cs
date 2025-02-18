using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models.DTOs;

public class GroupParentItemDTO
{
    public long Id { get; set; }
    [Required]
    public long GroupID { get; set; }
    [Required]
    public long ParentID { get; set; } 
}