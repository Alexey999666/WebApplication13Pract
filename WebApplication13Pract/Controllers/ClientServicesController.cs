using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication13Pract.Models;
using WebApplication13Pract.DTOs;

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

        // GET: api/ClientServices/Info - получение информации с связанными данными
        [HttpGet("Info")]
        public async Task<ActionResult<IEnumerable<ClientServiceInfoDTO>>> GetClientServiceInfoDTO()
        {
            // Загружаем связанные данные
            await _context.Clients.LoadAsync();
            await _context.Services.LoadAsync();

            // Получаем данные из БД и переводим в формат DTO
            return await _context.ClientServices
                .Include(cs => cs.Client)
                .Include(cs => cs.Service)
                .Select(p => new ClientServiceInfoDTO(p))
                .ToListAsync();
        }

        // GET: api/ClientServices - стандартный метод получения записей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientService>>> GetClientServices()
        {
            return await _context.ClientServices.ToListAsync();
        }

        // GET: api/ClientServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientService>> GetClientService(int id)
        {
            var clientService = await _context.ClientServices
                .Include(cs => cs.Client)
                .Include(cs => cs.Service)
                .FirstOrDefaultAsync(cs => cs.IdclientServices == id);

            if (clientService == null)
            {
                return NotFound($"Ошибка 404: Запись id={id} не обнаружена!");
            }

            return clientService;
        }

        // PUT: api/ClientServices/5 - редактирование с использованием DTO
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientService(int id, ClientServiceDTO clientServiceDTO)
        {
            // Проверяем соответствие ID в маршруте и в теле запроса
            if (id != clientServiceDTO.IdclientServices)
            {
                return BadRequest($"Ошибка 400: Несоответствие ID в маршруте и теле запроса!");
            }

            // Ищем исходную запись по IdclientServices
            var clientService = await _context.ClientServices.FindAsync(id);

            if (clientService == null)
            {
                return NotFound($"Ошибка 404: Запись id={id} не обнаружена!");
            }

            // Проверяем существование клиента
            var clientExists = await _context.Clients
                .AnyAsync(c => c.CardNumber == clientServiceDTO.ClientId);
            if (!clientExists)
            {
                return BadRequest("Ошибка: Клиент не найден");
            }

            // Проверяем существование услуги
            var serviceExists = await _context.Services
                .AnyAsync(s => s.Code == clientServiceDTO.ServiceId);
            if (!serviceExists)
            {
                return BadRequest("Ошибка: Услуга не найдена");
            }

            // Обновляем данные из DTO
            clientService.Update(clientServiceDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientServiceExists(id))
                {
                    return NotFound($"Ошибка 404: Запись id={id} не обнаружена!");
                }
                else
                {
                    return Conflict($"Ошибка 409: Запись id={id} заблокирована или изменена другим пользователем!");
                }
            }

            return NoContent();
        }

        // POST: api/ClientServices - добавление с использованием DTO
        [HttpPost]
        public async Task<ActionResult<ClientServiceDTO>> PostClientService(ClientServiceDTO clientServiceDTO)
        {
            // Проверяем существование клиента
            var clientExists = await _context.Clients
                .AnyAsync(c => c.CardNumber == clientServiceDTO.ClientId);
            if (!clientExists)
            {
                return BadRequest("Ошибка: Клиент не найден");
            }

            // Проверяем существование услуги
            var serviceExists = await _context.Services
                .AnyAsync(s => s.Code == clientServiceDTO.ServiceId);
            if (!serviceExists)
            {
                return BadRequest("Ошибка: Услуга не найдена");
            }

            // Для новой записи ID может быть 0 или другим значением, 
            // но Entity Framework сгенерирует новый ID при сохранении
            ClientService clientService = new ClientService(clientServiceDTO);

            _context.ClientServices.Add(clientService);
            await _context.SaveChangesAsync();

            // Возвращаем созданную запись
            return CreatedAtAction("GetClientService",
                new { id = clientService.IdclientServices },
                clientService);
        }

        // DELETE: api/ClientServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientService(int id)
        {
            var clientService = await _context.ClientServices.FindAsync(id);
            if (clientService == null)
            {
                return NotFound($"Ошибка 404: Запись id={id} не обнаружена!");
            }

            _context.ClientServices.Remove(clientService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientServiceExists(int id)
        {
            return _context.ClientServices.Any(e => e.IdclientServices == id);
        }
    }
}