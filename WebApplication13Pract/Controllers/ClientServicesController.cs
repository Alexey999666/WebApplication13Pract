using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication13Pract.DTOs;

using WebApplication13Pract.Models;

namespace WebApplication13Pract.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientServicesController : ControllerBase
    {
        private readonly SalonDb2Context _context;

        public ClientServicesController(SalonDb2Context context)
        {
            _context = context;
        }

        // GET: api/ClientServices/Info
        [HttpGet("Info")]
        public async Task<ActionResult<IEnumerable<ClientServiceInfoDTO>>> GetClientServicesInfo()
        {
            // Загружаем связанные данные
            await _context.Clients.LoadAsync();
            await _context.Services.LoadAsync();

            // Преобразуем в DTO
            var result = await _context.ClientServices
                .Select(cs => new ClientServiceInfoDTO(cs))
                .ToListAsync();

            return result;
        }

        // GET: api/ClientServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientService>>> GetClientServices()
        {
            return await _context.ClientServices.ToListAsync();
        }

        // GET: api/ClientServices/{clientId}/{appointmentDateTime}
        [HttpGet("{clientId}/{appointmentDateTime}")]
        public async Task<ActionResult<ClientService>> GetClientService(int clientId, DateTime appointmentDateTime)
        {
            var clientService = await _context.ClientServices
                .FirstOrDefaultAsync(cs => cs.ClientId == clientId && cs.AppointmentDateTime == appointmentDateTime);

            if (clientService == null)
            {
                return NotFound($"Запись не найдена.");
            }

            return clientService;
        }

        // PUT: api/ClientServices/{clientId}/{appointmentDateTime}
        [HttpPut("{clientId}/{appointmentDateTime}")]
        public async Task<IActionResult> PutClientService(int clientId, DateTime appointmentDateTime, ClientServiceDTO clientServiceDTO)
        {
            if (clientId != clientServiceDTO.ClientId || appointmentDateTime != clientServiceDTO.AppointmentDateTime)
            {
                return BadRequest("Данные в запросе и теле не совпадают.");
            }

            var clientService = await _context.ClientServices
                .FirstOrDefaultAsync(cs => cs.ClientId == clientId && cs.AppointmentDateTime == appointmentDateTime);

            if (clientService == null)
            {
                return NotFound($"Запись не найдена.");
            }

            // Проверяем существование клиента и услуги
            if (!_context.Clients.Any(c => c.CardNumber == clientServiceDTO.ClientId))
            {
                return BadRequest("Клиент не найден.");
            }
            if (!_context.Services.Any(s => s.Code == clientServiceDTO.ServiceId))
            {
                return BadRequest("Услуга не найдена.");
            }

            clientService.Update(clientServiceDTO);
            _context.Entry(clientService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientServiceExists(clientId, appointmentDateTime))
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

        // POST: api/ClientServices
        [HttpPost]
        public async Task<ActionResult<ClientService>> PostClientService(ClientServiceDTO clientServiceDTO)
        {
            // Проверяем существование клиента и услуги
            if (!_context.Clients.Any(c => c.CardNumber == clientServiceDTO.ClientId))
            {
                return BadRequest("Клиент не найден.");
            }
            if (!_context.Services.Any(s => s.Code == clientServiceDTO.ServiceId))
            {
                return BadRequest("Услуга не найдена.");
            }

            // Проверяем, не существует ли уже записи с такими ключами
            bool exists = await _context.ClientServices
                .AnyAsync(cs => cs.ClientId == clientServiceDTO.ClientId && cs.AppointmentDateTime == clientServiceDTO.AppointmentDateTime);
            if (exists)
            {
                return Conflict("Запись с такими данными уже существует.");
            }

            var clientService = new ClientService(clientServiceDTO);
            _context.ClientServices.Add(clientService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientService", new { clientId = clientService.ClientId, appointmentDateTime = clientService.AppointmentDateTime }, clientService);
        }

        // DELETE: api/ClientServices/{clientId}/{appointmentDateTime}
        [HttpDelete("{clientId}/{appointmentDateTime}")]
        public async Task<IActionResult> DeleteClientService(int clientId, DateTime appointmentDateTime)
        {
            var clientService = await _context.ClientServices
                .FirstOrDefaultAsync(cs => cs.ClientId == clientId && cs.AppointmentDateTime == appointmentDateTime);

            if (clientService == null)
            {
                return NotFound($"Запись не найдена.");
            }

            _context.ClientServices.Remove(clientService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientServiceExists(int clientId, DateTime appointmentDateTime)
        {
            return _context.ClientServices.Any(e => e.ClientId == clientId && e.AppointmentDateTime == appointmentDateTime);
        }
    }
}