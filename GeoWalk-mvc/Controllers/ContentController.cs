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

            return Ok("Content was deleted!");
        }

        [HttpPost("content")]
        public async Task<IActionResult> CreateInfo([FromBody] ContentInputModel inputModel)
        {

            var item = await _context.Contents.FirstOrDefaultAsync(x => x.Title == inputModel.title);
            var location = await _context.Locations.FindAsync(inputModel.locationId);

            if (item == null || location != null)
            {
                var content = new Content();

                content.Title = inputModel.title;
                content.Info = inputModel.info;
                content.Question = inputModel.question;
                content.Answers = inputModel.answers;
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
        public async Task<IActionResult> UpdateInfo([FromRoute]int id, [FromBody] ContentInputModel inputModel)
        {
            //var content = _context.Contents.Where(x => x.Id == id);
            var content = await _context.Contents.FindAsync(id);
           
            if (content == null)
            {
                return NotFound();
            }
            content.Title = inputModel.title;
            content.Info = inputModel.info;
            content.Question = inputModel.question;
            content.Answers = inputModel.answers;

            await _context.SaveChangesAsync();

            return Ok(content);
        }

        public class ContentInputModel
        {
            public int? locationId { get; set; }
            public string title { get; set; }
            public string info { get; set; }
            public string question { get; set; }
            public string answers { get; set; }
        }
    }
}

    