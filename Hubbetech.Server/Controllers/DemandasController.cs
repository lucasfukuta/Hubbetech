using Hubbetech.Server.Data;
using Hubbetech.Server.Models;
using Hubbetech.Shared.Constants;
using Hubbetech.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hubbetech.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DemandasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DemandasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandaDto>>> GetDemandas()
        {
            IQueryable<Demanda> query = _context.Demandas;

            if (User.IsInRole(Roles.Gestor))
            {
                // Gestor sees all
            }
            else
            {
                // Funcionario sees only assigned
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                query = query.Where(d => d.AssignedToUserId == userId);
            }

            var demandas = await query.Select(d => new DemandaDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                Status = d.Status,
                AssignedToUserId = d.AssignedToUserId
            }).ToListAsync();

            return Ok(demandas);
        }

        [HttpPost]
        public async Task<ActionResult<DemandaDto>> CreateDemanda(DemandaDto demandaDto)
        {
            var demanda = new Demanda
            {
                Title = demandaDto.Title,
                Description = demandaDto.Description,
                Status = demandaDto.Status,
                AssignedToUserId = demandaDto.AssignedToUserId
            };

            _context.Demandas.Add(demanda);
            await _context.SaveChangesAsync();

            demandaDto.Id = demanda.Id;

            return CreatedAtAction(nameof(GetDemandas), new { id = demanda.Id }, demandaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemanda(int id, DemandaDto demandaDto)
        {
            if (id != demandaDto.Id)
            {
                return BadRequest();
            }

            var demanda = await _context.Demandas.FindAsync(id);
            if (demanda == null)
            {
                return NotFound();
            }

            // Verify permissions: Only Gestor or the assigned user can update? 
            // For now, let's allow Gestor or the assigned user.
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole(Roles.Gestor) && demanda.AssignedToUserId != userId)
            {
                return Forbid();
            }

            demanda.Title = demandaDto.Title;
            demanda.Description = demandaDto.Description;
            demanda.Status = demandaDto.Status;
            demanda.AssignedToUserId = demandaDto.AssignedToUserId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandaExists(id))
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
        [Authorize(Roles = Roles.Gestor)]
        public async Task<IActionResult> DeleteDemanda(int id)
        {
            var demanda = await _context.Demandas.FindAsync(id);
            if (demanda == null)
            {
                return NotFound();
            }

            _context.Demandas.Remove(demanda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DemandaExists(int id)
        {
            return _context.Demandas.Any(e => e.Id == id);
        }

        // Add other methods (POST, PUT, DELETE) as needed
    }
}
