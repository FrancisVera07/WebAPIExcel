using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models.DTOs;

public class GroupItemDTO
{
    public long Id { get; set; }
    [Required]
    public string Key { get; set; }
    [Required]
    public string Concept { get; set; }
    [Required]
    public long ExcelFileID { get; set; }
}