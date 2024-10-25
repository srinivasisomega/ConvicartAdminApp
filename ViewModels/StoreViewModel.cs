using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConvicartAdminApp.ViewModels
{
    public class StoreViewModel
    {
        [Required]
        [MaxLength(255)]
        public string? ProductName { get; set; }

        [MaxLength(1000)]
        public string? ProductDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Carbs { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Proteins { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Vitamins { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Minerals { get; set; }

        [Required]
        public TimeSpan CookTime { get; set; }

        [Required]
        public TimeSpan PrepTime { get; set; }

        [MaxLength(20)]
        [RegularExpression("Easy|Medium|Hard")]
        public string? Difficulty { get; set; }

        public int? PreferenceId { get; set; }

        public string? imgUrl { get; set; }

        public IFormFile? ProductImage { get; set; } // For file upload

        [Range(1, 5)]
        public int? Rating { get; set; }
    }

}
