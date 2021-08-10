using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeService.Models
{
    public class Bike
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
