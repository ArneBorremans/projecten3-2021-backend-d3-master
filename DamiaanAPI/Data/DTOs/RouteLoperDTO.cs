using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DamiaanAPI.Models;

namespace DamiaanAPI.Data.DTOs
{
    public class RouteLoperDTO
    {
        [Required]
        public int RouteID { get; set; }
        [Required]
        public int LoperID { get; set; }

        public string TshirtMaat { get; set; }

        public int Zichtbaarheid { get; set; }

        public DateTime GeregistreerdOp { get; set; }

        public DateTime? StartDatumEnUur { get; set; }

        public DateTime? EindDatumEnUur { get; set; }

        public decimal? LaatsteLocatieLat { get; set; }

        public decimal? LaatsteLocatieLon { get; set; }

        public Dictionary<string, string> RouteNaam { get; set; }

    }
}
