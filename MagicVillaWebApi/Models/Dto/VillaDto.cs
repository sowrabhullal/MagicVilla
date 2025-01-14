using System.ComponentModel.DataAnnotations;

namespace MagicVillaWebApi.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public int sqft { get; set; }

        public int occupancy { get; set; }
    }
}
