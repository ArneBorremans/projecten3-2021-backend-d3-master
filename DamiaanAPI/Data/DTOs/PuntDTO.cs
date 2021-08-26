using DamiaanAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DamiaanAPI.Data.DTOs
{
    public class PuntDTO
    {
        public string Naam { get; set; }
        [Required]
        public decimal Lon { get; set; }
        [Required]
        public decimal Lat { get; set; }
        [Required]
        public string[] Faciliteiten { get; set; }
        public int? Id { get; set; }


        public PuntDTO(Punt punt)
        {
            this.Lon = punt.Lon;
            this.Lat = punt.Lat;
            this.Faciliteiten = punt.Faciliteiten.Select(f => f.ToString()).ToArray();
            this.Id = punt.ID;
            this.Naam = punt.Naam;
        }

        protected PuntDTO() { }
    }
}
