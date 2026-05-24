using System.ComponentModel.DataAnnotations;

namespace Cataloger.Core.Entities.Categories.Dto
{
    public class CategoryDto
    {
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
