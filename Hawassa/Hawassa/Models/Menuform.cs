 
using System.ComponentModel.DataAnnotations;

namespace Hawassa.Models
{
    public class Menuform
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";
        [Required, MaxLength(100)]
        public string Location { get; set; } = "";
        [Required, MaxLength(100)]

        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = "";
        [Required]
        public Decimal Price { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
