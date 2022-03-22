using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{

    public class Location
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string TypeOfLayer { get; set; }
        public virtual Walk Walk { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }

}
