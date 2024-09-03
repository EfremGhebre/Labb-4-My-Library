using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Labb_4_My_Library.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Author")]
        public string Author { get; set; }

        [DisplayName("Book Available")]
        public bool IsReturned { get; set; }

        public ICollection<Borrow> Borrows { get; set; }
    }
}
