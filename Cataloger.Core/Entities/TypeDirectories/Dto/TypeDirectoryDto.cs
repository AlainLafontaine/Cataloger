using System.ComponentModel.DataAnnotations;

namespace Cataloger.Core.Entities.TypeDirectories.Dto
{
    public class TypeDirectoryDto
    {
        public long TypeDirectoryId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
