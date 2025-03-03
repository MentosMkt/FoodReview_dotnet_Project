using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hawassa.Models
{
    public class AddMenu
    {
        public int? Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public string Location { get; set; } = "";

        public string Description { get; set; } = "";

        [Precision(16, 2)]
        public Decimal? Price { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        [MaxLength(100)]
        public DateTime CreatedAt { get; set; }

    }
}
