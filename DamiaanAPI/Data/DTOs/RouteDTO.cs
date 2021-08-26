using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DamiaanAPI.Data.DTOs
{
    public class RouteDTO
    {
        public int? Id { get; set; }
        [Required]
        public Dictionary<string, string> Naam { get; set; }
        public double Lengte { get; set; }
        public bool Openbaar { get; set; }
        public DateTime Start { get; set; }
        public DateTime Einde { get; set; }
        public Dictionary<string, string> Beschrijving { get; set; }
        public double Prijs { get; set; }

        public Object GeoJson { get; set; }

        public IEnumerable<PuntDTO> Punten { get; set; }
        //...
      
        public RouteDTO(Route route)
        {
            Naam = route.Naam;
            Lengte = route.Lengte;
            Openbaar = route.Openbaar;
            Start = route.Start;
            Einde = route.Einde;
            Beschrijving = route.Beschrijving;
            GeoJson = route.GeoJson;
            Punten = route.Punten.Select(p => new PuntDTO(p));
            Id = route.ID;
            Prijs = route.Prijs;
        }

        public RouteDTO() { }
    }
}
