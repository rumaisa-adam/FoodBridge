using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodBridge.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }

        [Required]
        public string ItemName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public int ItemTypeId { get; set; }
        public virtual ItemType? ItemType { get; set; }

        public int SuburbId { get; set; }
        public virtual Suburb? Suburb { get; set; }

        public int StorageId { get; set; }
        public virtual Storage? Storage { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime BestBeforeDate { get; set; }

        public string ImageData { get; set; } = string.Empty;
        public string ItemNotes { get; set; } = string.Empty;

        public string Status { get; set; } = "Available"; 

        public int PostedByUserId { get; set; }
        [ForeignKey("PostedByUserId")]
        public virtual User? PostedByUser { get; set; }

        public int? ClaimedByUserId { get; set; }
        [ForeignKey("ClaimedByUserId")]
        public virtual User? ClaimedByUser { get; set; }

        public DateTime? ClaimDate { get; set; }
    }
}