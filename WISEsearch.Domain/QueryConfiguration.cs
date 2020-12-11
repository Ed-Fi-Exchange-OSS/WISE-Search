using System.ComponentModel.DataAnnotations;

namespace WISEsearch.Domain
{
    public class QueryConfiguration
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public string TargetIndex { get; set; }

        [Required]
        public string Query { get; set; }


    }
}
