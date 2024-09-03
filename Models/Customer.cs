using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Labb_4_My_Library.Models
    {
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Phone Number")]

        public string PhoneNumber { get; set; }

        public ICollection<Borrow> Borrows { get; set; }
    }
    
}
