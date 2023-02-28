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

        // GET: api/shortenedLinks
        /// <summary>
        /// Listagem de Links Encurtados
        /// </summary>
        /// <returns>Lista de Links Encurtados</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            Log.Information("Endpoint - GET: api/shortenedLinks");

            var links = _context.Links;

            return Ok(links);
        }

        // GET: api/shortenedLinks/{id}
        /// <summary>
        /// Detalhes do Link Encurtado
        /// </summary>
        /// <param name="id">ID do Link Encurtado</param>
        /// <returns>Mostra um Link Encurtado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            Log.Information("Endpoint - GET: api/shortenedLinks/{id}");

            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound("Link Encurtado não encontrado.");

            return Ok(link);
        }

        // POST: api/shortenedLinks
        /// <summary>
        /// Cadastro de Link Encurtado
        /// </summary>
        /// <remarks>
        /// Requisição:
        /// {
        ///     "title": "GitHub Samuel B. Oldra",
        ///     "destinationLink" : "https://github.com/samuel-oldra"
        /// }
        /// </remarks>
        /// <param name="model">Dados do Link Encurtado</param>
        /// <returns>Objeto criado</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model)
        {
            Log.Information("Endpoint - POST: api/shortenedLinks");

            var domain = HttpContext.Request.Host.Value;

            var link = new ShortenedCustomLink(model.Title, model.DestinationLink, domain);

            // Verifica/atualiza links com o mesmo Code
            var sufixo = 0;
            var exists = true;
            while (exists)
            {
                exists = _context.Links.Any(l => l.Code == link.Code);
                if (!exists) continue;
                link.Code = string.Format("{0}-{1}", link.Title.Split(" ")[0].ToLower(), ++sufixo);
            }

            _context.Links.Add(link);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
        }

        // PUT: api/shortenedLinks/{id}
        /// <summary>
        /// Atualiza um Link Encurtado
        /// </summary>
        /// <remarks>
        /// Requisição:
        /// {
        ///     "title": "LinkedIn Samuel B. Oldra",
        ///     "destinationLink" : "https://www.linkedin.com/in/samuel-oldra/"
        /// }
        /// </remarks>
        /// <param name="id">ID do Link Encurtado</param>
        /// <param name="model">Dados do Link Encurtado</param>
        /// <response code="204">Sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model)
        {
            Log.Information("Endpoint - PUT: api/shortenedLinks/{id}");

            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound("Link Encurtado não encontrado.");

            link.Update(model.Title, model.DestinationLink);

            _context.Links.Update(link);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/shortenedLinks/{id}
        /// <summary>
        /// Deleta um Link Encurtado
        /// </summary>
        /// <param name="id">ID do Link Encurtado</param>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            Log.Information("Endpoint - DELETE: api/shortenedLinks/{id}");

            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound("Link Encurtado não encontrado.");

            _context.Links.Remove(link);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/shortenedLinks/{code}
        /// <summary>
        /// Redireciona o Link Encurtado
        /// </summary>
        /// <param name="code">Código do Link Encurtado</param>
        /// <returns>Redireciona um Link Encurtado</returns>
        /// <response code="404">Não encontrado</response>
        [HttpGet("/{code}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RedirectLink(string code)
        {
            Log.Information("Endpoint - GET: api/shortenedLinks/{code}");

            var link = _context.Links.SingleOrDefault(l => l.Code == code);

            if (link == null)
                return NotFound("Link Encurtado não encontrado.");

            return Redirect(link.DestinationLink);
        }
    }
}