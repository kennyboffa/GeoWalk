using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{
    public class Score
    {
      
        public int Id { get; set; }
        [Required]
        public int TotalScore { get; set; }
        public virtual User User { get; set; }
    }
}
