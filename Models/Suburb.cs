using System.ComponentModel.DataAnnotations;
namespace FoodBridge.Models
{
    public class Suburb
    {
        [Key]
        public int SuburbID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
