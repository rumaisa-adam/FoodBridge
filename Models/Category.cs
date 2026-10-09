using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodBridge.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<ItemType> ItemTypes { get; set; } = new List<ItemType>();

    }
}
