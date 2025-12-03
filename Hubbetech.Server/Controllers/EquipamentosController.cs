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
    public class EquipamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipamentoDto>>> GetEquipamentos()
        {
            return await _context.Equipamentos
                .Select(e => new EquipamentoDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    SerialNumber = e.SerialNumber,
                    Status = e.Status
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipamentoDto>> GetEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound();
            }

            return new EquipamentoDto
            {
                Id = equipamento.Id,
                Name = equipamento.Name,
                SerialNumber = equipamento.SerialNumber,
                Status = equipamento.Status
            };
        }

        [HttpPost]
        public async Task<ActionResult<EquipamentoDto>> PostEquipamento(EquipamentoDto equipamentoDto)
        {
            var equipamento = new Equipamento
            {
                Name = equipamentoDto.Name,
                SerialNumber = equipamentoDto.SerialNumber,
                Status = equipamentoDto.Status
            };

            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();

            equipamentoDto.Id = equipamento.Id;

            return CreatedAtAction("GetEquipamento", new { id = equipamento.Id }, equipamentoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipamento(int id, EquipamentoDto equipamentoDto)
        {
            if (id != equipamentoDto.Id)
            {
                return BadRequest();
            }

            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento == null)
            {
                return NotFound();
            }

            equipamento.Name = equipamentoDto.Name;
            equipamento.SerialNumber = equipamentoDto.SerialNumber;
            equipamento.Status = equipamentoDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipamentoExists(id))
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
        public async Task<IActionResult> DeleteEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento == null)
            {
                return NotFound();
            }

            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipamentoExists(int id)
        {
            return _context.Equipamentos.Any(e => e.Id == id);
        }
    }
}
