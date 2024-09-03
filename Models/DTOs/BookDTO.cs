using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Labb_4_My_Library.Models.DTOs
{
    public class BookDTO
    { 
        public string Title { get; set; }
        
        public string Author { get; set; }

        public bool IsReturned { get; set; }

    }
}
