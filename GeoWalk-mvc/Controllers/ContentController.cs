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
            //_context.Answers.Remove()
            await _context.SaveChangesAsync();

            return Ok("Content was deleted!");
        }

        [HttpPost("content")]
        public async Task<IActionResult> CreateContent([FromBody] ContentInputModel inputModel)
        {

            //var item = await _context.Contents.FirstOrDefaultAsync(x => x.Title == inputModel.title);
            var location = await _context.Locations.FindAsync(inputModel.locationId);

            if (location != null)
            {
                if (inputModel.type == "info")
                {
                    var content = new InfoContent();

                    content.Title = inputModel.title;
                    content.Info = inputModel.info;
                    content.Location = location;

                    _context.Contents.Add(content);

                    await _context.SaveChangesAsync();

                }
                else if (inputModel.type == "questionanswer")
                { 
                var content = new QuestionAnswer();

                content.Title = inputModel.title;
                content.Question = inputModel.question;
                content.Answers = inputModel.answers;
                content.Location = location;


                    _context.Contents.Add(content);

                    await _context.SaveChangesAsync();
                }


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
        /// <summary>
        /// Get one Walk by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("content/{id}/info")]
        public async Task<IActionResult> GetInfoContent(int id)
        {
            var content = await _context.Contents.FirstOrDefaultAsync(x => x.Id == id);

            if (content is null)
            {
                return NotFound();
            }


            return Ok(content);
        }

        /// <summary>
        /// Get one Walk by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("content/{id}/questionanswer")]
        public async Task<IActionResult> GetQuestionContent(int id)
        {
            var content = await _context.Answers
                .Where(x => x.QuestionAnswer.Id == id).ToListAsync();
           
            //var answers = await _context.Answers.FirstOrDefaultAsync(x => x.QuestionAnswer.Id == id);

            if (content is null)
            {
                return NotFound();
            }
            

            return Ok(content);
        }


        [HttpPut("content/{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] ContentInputModel inputModel)
        {
            //var content = _context.Contents.Where(x => x.Id == id);
            var item = await _context.Contents.FindAsync(id);
           
            if (item == null)
            {
                return NotFound();
            }
            if (item.Type == "info")
            {
                var content = new InfoContent();

                content = (InfoContent)await _context.Contents.FindAsync(id);

                content.Title = inputModel.title;
                content.Info = inputModel.info;
            }
            else if (item.Type == "questionanswer")
            {
                var content = new QuestionAnswer();

                content = (QuestionAnswer)await _context.Contents.FindAsync(id);


                content.Title = inputModel.title;
                content.Question = inputModel.question;
                content.Answers = inputModel.answers;
            }


            await _context.SaveChangesAsync();

            return Ok(item);
        }

        public class ContentInputModel
        {
            public int? locationId { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string info { get; set; }
            public string question { get; set; }
            public IEnumerable<Answer> answers { get; set; }
        }
    }
}

    