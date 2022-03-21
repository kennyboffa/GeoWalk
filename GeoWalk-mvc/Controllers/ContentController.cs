using GeoWalk_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeoWalk_mvc.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly MyDbContext _context;

        public ContentController(ILogger<ContentController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        [HttpDelete("content/{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            var content = await _context.Contents.FindAsync(id);

            if (content == null)
            {
                return NotFound();
            }

            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();

            return Ok("Content was deleted");
        }

        [HttpPost("content/{title} {info} {locationId}")]
        public async Task<IActionResult> CreateInfo(string title, string info, int locationId)
        {

            var item = await _context.Contents.FirstOrDefaultAsync(x => x.Title == title);
            var location = await _context.Locations.FindAsync(locationId);

            if (item == null || location != null)
            {
                var content = new Content();

                content.Title = title;
                content.Info = info;
                content.Location = location;

                _context.Contents.Add(content);


                await _context.SaveChangesAsync();

            }
            else
            {
                return BadRequest("Something went wrong!");
            }

            return Ok("Content saved!");

        }
        /// <summary>
        /// Get one Walk by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("content/{id}")]
        public async Task<IActionResult> GetContent(int id)
        {
            var content = await _context.Contents.FirstOrDefaultAsync(x => x.Id == id);

            if (content is null)
            {
                return NotFound();
            }


            return Ok(content);
        }

        [HttpPut("content/{id}")]
        public async Task<IActionResult> UpdateInfo([FromRoute]int id, [FromBody] UpdateInfoInputModel inputModel)
        {
            //var content = _context.Contents.Where(x => x.Id == id);
            var content = await _context.Contents.FindAsync(id);
           
            if (content == null)
            {
                return NotFound();
            }
            content.Title = inputModel.title;
            content.Info = inputModel.info;

            await _context.SaveChangesAsync();

            return Ok(content);
        }

        public class UpdateInfoInputModel
        { 
            public string title { get; set; }
            public string info { get; set; }
        }
    }
}
