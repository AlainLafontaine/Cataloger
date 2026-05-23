using System.ComponentModel.DataAnnotations;

namespace Cataloger.Core.Entities.SystemsParameters.Dto
{
    public class SystemParameterDto
    {
        public long SystemParameterId { get; set; }

        [Required]
        [MaxLength(32)]
        public string Section { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [MaxLength(128)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(256)]
        public string? ValString { get; set; } = null;
        
        public long? ValLong { get; set; } = null;
        
        public double? ValDouble { get; set; } = null;
        
        public DateTime? ValDate { get; set; } = null;
        
        public bool? ValBooleen { get; set; } = null;

        public char? ValChar { get; set; } = null;
    }
}