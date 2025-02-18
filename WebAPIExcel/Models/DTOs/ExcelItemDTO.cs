using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models.DTOs;

public class ExcelItemDTO
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
}