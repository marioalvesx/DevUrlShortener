using DevUrlShortener.Entities;
using DevUrlShortener.Models;
using DevUrlShortener.Persistence;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevUrlShortener.Controllers
{
    [ApiController]
    [Route("api/shortenedLinks")]
    public class ShortenedLinksController : ControllerBase
    {
        private readonly DevURLShortenerDbContext _context;

        public ShortenedLinksController(DevURLShortenerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() {
            Log.Information("GetAll is called!");
            return Ok(_context.Links);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if(link == null) {
                return NotFound();
            }

            return Ok(link);
        }

        /// <summary>
        /// Register shortened link
        /// </summary>
        /// <remarks>
        /// { "title": "my-github link", "destinationLink": "https://github.com/marioalvesx" }
        /// </remarks>
        /// <param name="model">Link data</param>
        /// <returns>Object created</returns>
        /// <response code="201">Success</response>
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
        public IActionResult Delete(int id) {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if(link == null)
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