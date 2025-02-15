using System.ComponentModel.DataAnnotations;

namespace WebAPIExcel.Models;

public class ExcelItem
{
    public long Id { get; private set; }
    [Required]
    public String? Name { get; set; }
    public DateTime? CreateTimestamp { get; private set; } = DateTime.Now;
    public DateTime? UpdateTimestamp { get; private set; } = DateTime.Now;

    public void Update(string name)
    {
        Name = name;
        UpdateTimestamp = DateTime.Now;
    }
}