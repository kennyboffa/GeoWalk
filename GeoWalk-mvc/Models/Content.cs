using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{
    public class Content
    {
        
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Info { get; set; }
        public string Question { get; set; }
        public string Answers { get; set; }
        public virtual Location Location { get; set; }

    }

}
