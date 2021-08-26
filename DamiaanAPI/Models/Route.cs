using DamiaanAPI.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class Route
    {
        #region Fields & properties
        private double lengte;
        private DateTime start;

        public int ID { get; set; }
        public Dictionary<string, string> Naam { get; set; }
        public double Lengte
        {
            get => lengte;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid lengte");
                }
                lengte = value;
            }
        } //Lengte in km's
        public string GeoJson { get; set; }
        public List<Punt> Punten { get; set; }
        public DateTime Start
        {
            get => start;
            set
            {
                if (value < DateTime.Now)
                {
                    throw new ArgumentException("Start must be a future date");
                }
                start = value;
            }
        }
        public DateTime Einde { get; set; }
        public Dictionary<string, string> Beschrijving { get; set; }
        public bool Openbaar { get; set; } //Kan voor ingeschreven worden
        public ICollection<RouteLoper> Lopers { get; set; }
        public double Prijs { get; set; }
        public ICollection<string> Afbeeldingen { get; set; }
        #endregion

        #region Constructors
        public Route()
        {
            Punten = new List<Punt>();
            Lopers = new HashSet<RouteLoper>();
            Beschrijving = new Dictionary<string, string>();
            Afbeeldingen = new HashSet<string>();
            Naam = new Dictionary<string, string>();
        }

        public Route(RouteDTO dto) : this()
        {
            if (dto.GeoJson == null)
                throw new ArgumentException();
            SetFromDto(dto);            
        }
        #endregion


        #region Methods
        public void SetFromDto(RouteDTO dto)
        {
            GeoJson = dto.GeoJson != null ? dto.GeoJson.ToString() : GeoJson;
            Naam = dto.Naam;
            Lengte = dto.Lengte;
            Openbaar = dto.Openbaar;
            Start = dto.Start;
            Einde = dto.Einde;
            Beschrijving = dto.Beschrijving;
            Prijs = dto.Prijs;
            if (dto.Punten != null)
            {
                foreach (PuntDTO PuntDTO in dto.Punten)
                {
                    Punt punt = new Punt(PuntDTO);
                    Punten.Add(punt);
                }
            }
        }
        public RouteLoper AddLoper(Loper loper, string tshirtMaat, Zichtbaarheid zichtbaarheid)
        {
            var inschrijving = new RouteLoper(this, loper);
            inschrijving.TshirtMaat = tshirtMaat;
            inschrijving.Zichtbaarheid = zichtbaarheid;
            loper.GeregistreerdeRoutes.Add(inschrijving);
            Lopers.Add(inschrijving);
            return inschrijving;
        }

        public void RemoveLoper(Loper loper)
        {
            var inschrijving = Lopers.First(i => i.LoperID == loper.ID);
            Lopers.Remove(inschrijving);
            loper.GeregistreerdeRoutes.Remove(inschrijving);
        }

        public bool IsIngeschreven(Loper loper)
        {
            return Lopers.Any(i => i.LoperID == loper.ID);
        }
    } 
    #endregion
}
