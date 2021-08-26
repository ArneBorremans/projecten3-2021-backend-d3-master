using System;
using System.Collections.Generic;
using DamiaanAPI.Models;

namespace DamiaanAPI.Data.DTOs
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gemeente { get; set; }

        public string Email { get; set; }

        public List<RouteLoperDTO> Inschrijvingen { get; set; }
    }
}
