using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
