using Hubbetech.Server.Data;
using Hubbetech.Server.Models;
using Hubbetech.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hubbetech.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LinksEmpresaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LinksEmpresaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinkEmpresaDto>>> GetLinksEmpresa()
        {
            return await _context.LinksEmpresa
                .Select(l => new LinkEmpresaDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Url = l.Url
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LinkEmpresaDto>> GetLinkEmpresa(int id)
        {
            var linkEmpresa = await _context.LinksEmpresa.FindAsync(id);

            if (linkEmpresa == null)
            {
                return NotFound();
            }

            return new LinkEmpresaDto
            {
                Id = linkEmpresa.Id,
                Title = linkEmpresa.Title,
                Url = linkEmpresa.Url
            };
        }

        [HttpPost]
        public async Task<ActionResult<LinkEmpresaDto>> PostLinkEmpresa(LinkEmpresaDto linkEmpresaDto)
        {
            var linkEmpresa = new LinkEmpresa
            {
                Title = linkEmpresaDto.Title,
                Url = linkEmpresaDto.Url
            };

            _context.LinksEmpresa.Add(linkEmpresa);
            await _context.SaveChangesAsync();

            linkEmpresaDto.Id = linkEmpresa.Id;

            return CreatedAtAction("GetLinkEmpresa", new { id = linkEmpresa.Id }, linkEmpresaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLinkEmpresa(int id, LinkEmpresaDto linkEmpresaDto)
        {
            if (id != linkEmpresaDto.Id)
            {
                return BadRequest();
            }

            var linkEmpresa = await _context.LinksEmpresa.FindAsync(id);
            if (linkEmpresa == null)
            {
                return NotFound();
            }

            linkEmpresa.Title = linkEmpresaDto.Title;
            linkEmpresa.Url = linkEmpresaDto.Url;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkEmpresaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkEmpresa(int id)
        {
            var linkEmpresa = await _context.LinksEmpresa.FindAsync(id);
            if (linkEmpresa == null)
            {
                return NotFound();
            }

            _context.LinksEmpresa.Remove(linkEmpresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinkEmpresaExists(int id)
        {
            return _context.LinksEmpresa.Any(e => e.Id == id);
        }
    }
}
