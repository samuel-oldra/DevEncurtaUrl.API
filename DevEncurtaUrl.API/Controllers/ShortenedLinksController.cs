using DevEncurtaUrl.API.Entities;
using DevEncurtaUrl.API.Models;
using DevEncurtaUrl.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevEncurtaUrl.API.Controllers
{
    [ApiController]
    [Route("api/shortenedLinks")]
    public class ShortenedLinksController : ControllerBase
    {
        private readonly DevEncurtaUrlDbContext _context;

        public ShortenedLinksController(DevEncurtaUrlDbContext context)
            => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            Log.Information("Endpoint - GET: api/shortenedLinks");

            var links = _context.Links;

            return Ok(links);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound();

            return Ok(link);
        }

        /// <summary>
        /// Cadastrar um link encurtado
        /// </summary>
        /// <remarks>
        /// { "title": "ultimo-artigo Blog", "destinationLink" : "https://www.luisdev.com.br/2023/02/05/atributos-no-c-como-criar-e-aplicar-em-seu-projeto" }
        /// </remarks>
        /// <param name="model">Dados de link</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso!</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model)
        {
            var domain = HttpContext.Request.Host.Value;

            var link = new ShortenedCustomLink(model.Title, model.DestinationLink, domain);

            _context.Links.Add(link);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = link.Id }, link);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model)
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound();

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound();

            _context.Links.Remove(link);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("/{code}")]
        public IActionResult RedirectLink(string code)
        {
            var link = _context.Links.SingleOrDefault(l => l.Code == code);

            if (link == null)
                return NotFound();

            return Redirect(link.DestinationLink);
        }
    }
}