using WebApplication13Pract.Models;
using WebApplication13Pract.DTOs;
namespace WebApplication13Pract.DTOs
{
    public partial class ClientServiceInfoDTO
    {
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public string LastName { get; set; } // Фамилия клиента
        public string Phone { get; set; } // Телефон клиента
        public string ServiceName { get; set; } // Название услуги
        public decimal Price { get; set; } // Цена услуги
        public DateTime AppointmentDateTime { get; set; }


        public ClientServiceInfoDTO() { }
        // Конструктор для передачи данных из сущности в DTO
        public ClientServiceInfoDTO(ClientService value)
        {
            ClientId = value.ClientId;
            ServiceId = value.ServiceId;
            LastName = value.Client?.LastName ?? "Не указано";
            Phone = value.Client?.Phone ?? "Не указано";
            ServiceName = value.Service?.Name ?? "Не указано";
            Price = value.Service?.Price ?? 0;
            AppointmentDateTime = value.AppointmentDateTime;
        }

      
    }
}
