using System;
using System.Collections.Generic;
using WebApplication13Pract.DTOs;
using WebApplication13Pract.Models;
namespace WebApplication13Pract.DTOs
{
    public partial class ClientServiceDTO
    {
        public int IdclientServices { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
