using GeoWalk_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeoWalk_mvc.Controllers
{

    public class WalkController : ControllerBase
    {
        private readonly ILogger<WalkController> _logger;
        private readonly MyDbContext _context;

        public WalkController(ILogger<WalkController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        /// <summary>
        /// Deletes a specific Walk
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("walk/{id}")]
        public async Task<IActionResult> DeleteWalk(int id)
        {
            var item = await _context.Walks.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            _context.Walks.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
        /// <summary>
        /// Adds a Walk
        /// </summary>
        /// <returns></returns>
        [HttpPost("walk/{name}")]
        public async Task<IActionResult> AddWalk(string name)
        {
            var item = await _context.Walks.FirstOrDefaultAsync(x => x.WalkName == name);

            if (item is null)
            {
                Walk walk = new Walk();


                walk.WalkName = name;

                _context.Walks.Add(walk);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("That Id already exists!");
            }


            return Ok("Saved!");
        }

        /// <summary>
        /// Get all Walks
        /// </summary>
        /// <returns></returns>
        [HttpGet("walk/")]
        public async Task<IActionResult> GetAllWalks()
        {
            var listWalks = await _context.Walks.Include(x => x.Locations).ToListAsync();

            //List<Walk> allWalks = new List<Walk>();
            //allWalks = await _context.Walks.FindAsync();

            if (listWalks is null)
            {
                return NotFound();
            }


            return Ok(listWalks);
        }

        /// <summary>
        /// Get all Walks
        /// </summary>
        /// <returns></returns>
        [HttpGet("walk/{id}")]
        public async Task<IActionResult> GetWalk(int id)
        {
            var walk = await _context.Walks.Where(x => x.Id == id).Include(x => x.Locations).FirstOrDefaultAsync();

            //List<Walk> allWalks = new List<Walk>();
            //allWalks = await _context.Walks.FindAsync();

            if (walk is null)
            {
                return NotFound();
            }


            return Ok(walk);
        }

        [HttpPut("walk/{id}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] int id, [FromBody] UpdateWalkInputModel inputModel)
        {
            //var content = _context.Contents.Where(x => x.Id == id);
            var walk = await _context.Walks.FindAsync(id);

            if (walk == null)
            {
                return NotFound();
            }
            walk.WalkName = inputModel.walkName;

            await _context.SaveChangesAsync();

            return Ok(walk);

        }
        public class UpdateWalkInputModel
        {
            public string walkName { get; set; }
        }
    }
}