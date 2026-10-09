using System.ComponentModel.DataAnnotations;
namespace FoodBridge.Models
{
    public class ItemType
    {
        [Key]
        public int ItemTypeID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }
    }
}
