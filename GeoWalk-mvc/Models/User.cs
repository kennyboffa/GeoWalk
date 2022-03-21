using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{
    public class User
    {
     
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }


    }
}
