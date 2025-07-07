using System.ComponentModel.DataAnnotations;

namespace SachisGallery.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, 1000, ErrorMessage ="Display order should be between 1 - 1000")]
        public int DisplayOrder { get; set; }
    }
}
