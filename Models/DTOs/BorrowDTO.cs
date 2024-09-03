using System.ComponentModel;

namespace Labb_4_My_Library.Models.DTOs
{
    public class BorrowDTO
    {
        [DisplayName("Book Title")]
        public int BookId { get; set; }
        [DisplayName("Customer Name")]
        public int CustomerId { get; set; }
        [DisplayName("Borrow Date")]
        public DateTime BorrowDate { get; set; }
        [DisplayName("Return Date")]
        public DateTime? ReturnDate { get; set; }
    }
}
