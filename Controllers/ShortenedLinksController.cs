using DevUrlShortener.Entities;
using DevUrlShortener.Models;
using DevUrlShortener.Persistence;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetById(int id) 
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if(link == null) {
                return NotFound();
            }

            return Ok(link);
        }

        [HttpPost]
        public IActionResult Post(AddOrUpdateShortenedLinkModel model)
        {
            var link = new ShortenedCustomLink(model.Title, model.DestinationLink);

            _context.Add(link);

            return CreatedAtAction("GetById", new { id = link.Id }, link);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AddOrUpdateShortenedLinkModel model)
        {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound();

            link.Update(model.Title, model.DestinationLink);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            var link = _context.Links.SingleOrDefault(l => l.Id == id);

            if(link == null)
                return NotFound();

            _context.Links.Remove(link);
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