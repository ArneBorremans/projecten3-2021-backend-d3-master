using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DamiaanAPI.Data.DTOs
{
    public class LoperDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gemeente { get; set; }

        public string Email { get; set; }
    }
}