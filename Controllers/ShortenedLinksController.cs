using DevEncurtaUrl.API.Entities;
using DevEncurtaUrl.API.Models;
using DevEncurtaUrl.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevEncurtaUrl.API.Controllers
{
    [ApiController]
    [Route("api/shortenedLinks")]
    public class ShortenedLinksController : ControllerBase
    {
        private readonly DevEncurtaUrlDbContext _context;

        public ShortenedLinksController(DevEncurtaUrlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok(_context.Links);
        }

        // api/shortenedLinks/1 GET
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null) {
                return NotFound();
            }

            return Ok(link);
        }

        [HttpPost]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model) {
            var link = new ShortenedCustomLink(model.Title, model.DestinationLink);

            _context.Links.Add(link);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = link.Id }, link);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model) {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null) {
                return NotFound();
            }

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);
            _context.SaveChanges();

            return NoContent();
        }

        // api/shortenedLinks/123
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null) {
                return NotFound();
            }

            _context.Links.Remove(link);
            _context.SaveChanges();
            
            return NoContent();
        }

        // localhost:3000/ultimo-artigo
        [HttpGet("/{code}")]
        public IActionResult RedirectLink(string code) {
            var link = _context.Links.SingleOrDefault(l => l.Code == code);

            if (link == null) {
                return NotFound();
            }

            return Redirect(link.DestinationLink);
        }
    }
}