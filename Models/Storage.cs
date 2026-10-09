using System.ComponentModel.DataAnnotations;
namespace FoodBridge.Models
{
    public class Storage
    {
        [Key]
        public int StorageID { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
