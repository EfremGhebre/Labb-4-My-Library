using System.ComponentModel;

namespace Labb_4_My_Library.Models
{
    public class Borrow
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } 

        public int BookId { get; set; }
        public Book Book { get; set; }

        [DisplayName("Borrow Date")]
        public DateTime BorrowDate { get; set; }
        [DisplayName("Return Date")]
        public DateTime? ReturnDate { get; set; }
    }
}
