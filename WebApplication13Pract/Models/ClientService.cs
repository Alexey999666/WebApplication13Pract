using System;
using System.Collections.Generic;
using WebApplication13Pract.DTOs;

namespace WebApplication13Pract.Models;

public partial class ClientService
{
    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    // Конструктор для создания из DTO
    public ClientService(ClientServiceDTO dto)
    {
        ClientId = dto.ClientId;
        ServiceId = dto.ServiceId;
        AppointmentDateTime = dto.AppointmentDateTime;
    }

    // Метод для обновления из DTO
    public void Update(ClientServiceDTO dto)
    {
        ClientId = dto.ClientId;
        ServiceId = dto.ServiceId;
        AppointmentDateTime = dto.AppointmentDateTime;
    }

    // Пустой конструктор
    public ClientService() { }
}
