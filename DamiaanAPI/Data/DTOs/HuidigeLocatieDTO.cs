using System;
namespace DamiaanAPI.Data.DTOs
{
    public class HuidigeLocatieDTO
    {
        public decimal? Lat { get; set; }
        public decimal? Lon { get; set; }
        public int RouteId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        public HuidigeLocatieDTO()
        {

        }
    }
}
