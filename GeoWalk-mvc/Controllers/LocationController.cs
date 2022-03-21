using GeoWalk_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeoWalk_mvc.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> _logger;
            private readonly MyDbContext _context;

        public LocationController(ILogger<LocationController> logger, MyDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Deletes a specific Walk
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("location/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var item = await _context.Locations.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            _context.Locations.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPost("location")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationInputModel inputModel)
        {
            var walk = await _context.Walks.FindAsync(inputModel.walkId);

            if(walk == null)
            {
                return NotFound();
            }

            var location = new Location(); 

            location.Walk = walk;
            location.Title = inputModel.title;
            location.Latitude = inputModel.latitude;
            location.Longitude = inputModel.longitude;

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return Ok(location);
        }

        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetLocationContent(int id)

        {
            var location = await _context.Locations.Include(x => x.Contents).FirstOrDefaultAsync(x => x.Id == id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPut("location/{id}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] int id, [FromBody] LocationInputModel inputModel)
        {
            //var content = _context.Contents.Where(x => x.Id == id);
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }
            location.Title = inputModel.title;
            location.Latitude = inputModel.latitude;
            location.Longitude = inputModel.longitude;
            await _context.SaveChangesAsync();

            return Ok(location);
        }


        public class LocationInputModel
        {
            public int? walkId { get; set; }
            public string title { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
        }
    }
}
