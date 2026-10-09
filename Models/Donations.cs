using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FoodBridge.Models
{
    public class Donations
    {
        [Key]
        public int DonationID { get; set; }
        public int UserID { get; set; }
        public virtual User? User { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; } = DateTime.UtcNow;
    }
}
