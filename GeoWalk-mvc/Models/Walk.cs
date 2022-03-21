using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{
    public class Walk
    {

        public int Id { get; set; }
        [Required]
        public string WalkName { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

 
    }
}
