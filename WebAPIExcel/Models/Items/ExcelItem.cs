    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    namespace WebAPIExcel.Models.Items;

    public class ExcelItem
    {
        public long Id { get; set; }
        [Required]
        public String Name { get; set; }
        public DateTime CreateTimestamp { get; set; } = DateTime.Now;
        public DateTime UpdateTimestamp { get; set; } = DateTime.Now;
        
        // Relación GroupItems con ExcelItems 
        [JsonIgnore]
        public ICollection<GroupItem> GroupItems { get; set; } = new List<GroupItem>();
        
        public void Update(string name)
        {
            Name = name;
            UpdateTimestamp = DateTime.Now;
        }
    }