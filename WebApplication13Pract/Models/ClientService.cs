using System;
using System.Collections.Generic;
using WebApplication13Pract.DTOs;

namespace WebApplication13Pract.Models;

public partial class ClientService
{
    public int IdclientServices { get; set; }

    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;


    public ClientService() { }

    // Конструктор для передачи данных из DTO в сущность
    public ClientService(ClientServiceDTO value)
    {
        IdclientServices = value.IdclientServices;
        ClientId = value.ClientId;
        ServiceId = value.ServiceId;
        AppointmentDateTime = value.AppointmentDateTime;
    }

    public void Update(ClientServiceDTO value)
    {
        ClientId = value.ClientId;
        ServiceId = value.ServiceId;
        AppointmentDateTime = value.AppointmentDateTime;
    }

}
