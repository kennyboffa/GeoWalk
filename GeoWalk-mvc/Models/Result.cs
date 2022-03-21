using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{

        public class Result
        {
            
            public int Id { get; set; }
            [Required]
            public string? Message { get; set; }
            public virtual Score Score { get; set; }
        }
    
}
