using System.ComponentModel.DataAnnotations;

namespace Cataloger.Core.Entities.Directories.Dto
{
    public class DirectoryDto
    {
        public long DirectoryId { get; set; }

        [Required]
        [MaxLength(512)]
        public string Name { get; set; } = string.Empty;

        public long TypeDirectoryId { get; set; }

        public long CategoryId { get; set; }
    }
}
