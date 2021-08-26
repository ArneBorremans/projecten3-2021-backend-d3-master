using DamiaanAPI.Data.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class Punt
    {
        #region Fields
        private static readonly decimal MIN_LAT = -90;
        private static readonly decimal MAX_LAT = 90;
        private static readonly decimal MIN_LON = -180;
        private static readonly decimal MAX_LON = 180;
        private static readonly string FACILITEITEN_SEPARATOR = "|";

        private ICollection<string> faciliteiten;
        private decimal _lon;
        private decimal _lat;
        #endregion

        #region Properties
        public int ID { get; set; }
        public string Naam { get; set; }
        public decimal Lon
        {
            get => _lon;
            set
            {
                if (value < MIN_LON || value > MAX_LON)
                {
                    throw new ArgumentException("Invalid longitude");
                }
                _lon = value;
            }
        }
        public decimal Lat
        {
            get => _lat;
            set
            {
                if (value < MIN_LAT || value > MAX_LAT)
                {
                    throw new ArgumentException("Invalid latitude");
                }
                _lat = value;
            }
        }
        public IEnumerable<string> Faciliteiten
        {
            get => faciliteiten;
            set
            {
                this.faciliteiten = new HashSet<string>();
                foreach (var faciliteit in value)
                {
                    VoegFaciliteitToe(faciliteit);
                }
            }
        }
        #endregion

        #region Constructors
        public Punt(string Naam, decimal Lon, decimal Lat, IEnumerable<string> faciliteiten) : this()
        {
            this.Naam = Naam;
            this.Lon = Lon;
            this.Lat = Lat;
            foreach (var faciliteit in faciliteiten)
            {
                this.faciliteiten.Add(faciliteit);
            }
        }

        public Punt()
        {
            faciliteiten = new HashSet<string>();
        }

        public Punt(PuntDTO dto): this()
        {
               SetFromDto(dto);
        }
        #endregion

        #region Methods
        public void SetFromDto(PuntDTO dto)
        {
            if (dto.Faciliteiten.Any(f => f.Contains(FACILITEITEN_SEPARATOR)))
                throw new ArgumentException(FACILITEITEN_SEPARATOR + " is niet toegestaan in faciliteiten");
            Lat = dto.Lat;
            Lon = dto.Lon;
            Naam = dto.Naam;
            Faciliteiten = dto.Faciliteiten.ToHashSet();
        }
        public void VoegFaciliteitToe(string faciliteit)
        {
            if (faciliteit.Contains(FACILITEITEN_SEPARATOR))
            {
                throw new ArgumentException($"Faciliteiten mogen geen {FACILITEITEN_SEPARATOR} bevatten");
            }
            this.faciliteiten.Add(faciliteit);
        } 
        #endregion
    }

}
