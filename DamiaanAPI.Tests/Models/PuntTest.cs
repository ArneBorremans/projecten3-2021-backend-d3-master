using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace DamiaanAPI.Tests.Models
{
    public class PuntTest
    {
        #region TestData
        private readonly Punt _punt;
        private readonly int puntID = 1;
        private readonly string naam = "test";
        private readonly decimal lon = 75;
        private readonly decimal lat = 25;
        private readonly List<string> faciliteiten = new List<string>
        {
            "test",
            "test2",
            "test3"
        };
        private readonly string illegalFaciliteit = "test | test";
        private readonly string legalFaciliteit = "test123";
        #endregion

        #region Constructor
        public PuntTest()
        {
            _punt = new Punt( naam, lon, lat, faciliteiten);
        }
        #endregion

        #region Test Getters and Setters
        [Fact]
        public void testGettersAndSetters_ReturnsCorrectValues()
        {
           // Assert.Equal(_punt.ID, puntID);
            Assert.Equal(_punt.Naam, naam);
            Assert.Equal(_punt.Lon, lon);
            Assert.Equal(_punt.Lat, lat);
            Assert.Equal(_punt.Faciliteiten, faciliteiten);
        }
        #endregion

        #region VoegFaciliteitToe
        [Fact]
        public void voegFaciliteitToe_ContainsIllegalCharacter_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _punt.VoegFaciliteitToe(illegalFaciliteit));
        }

        [Fact]
        public void voegFaciliteitToe_SuccesfullyAdded_CountIncreases()
        {
            int currentCount = _punt.Faciliteiten.Count();

            _punt.VoegFaciliteitToe(legalFaciliteit);

            Assert.Equal(currentCount + 1, _punt.Faciliteiten.Count());
        }
        #endregion

        #region Commented
        /*private static readonly decimal MIN_LAT = -90;
        private static readonly decimal MAX_LAT = 90;
        private static readonly decimal MIN_LON = -180;
        private static readonly decimal MAX_LON = 180;
        private static readonly decimal PRECISION = 0.000000000000001M;
        
        #region Lat
          [Fact]
        public void Lat_OnderMinLat_GooitException()
        {
            var lat = MIN_LAT - PRECISION;
            Assert.Throws<ArgumentException>(() => _punt.Lat = lat);
        }

        [Fact]
        public void Lat_BovenMaxLat_GooitException()
        {
            var lat = MAX_LAT + PRECISION;
            Assert.Throws<ArgumentException>(() => _punt.Lat = lat);
        }
        #endregion

        #region Lon
        [Fact]
        public void Lon_OnderMinLon_GooitException()
        {
            var lon = MIN_LON - PRECISION;
            Assert.Throws<ArgumentException>(() => _punt.Lon = lon);
        }

        [Fact]
        public void Lon_BovenMaxLon_GooitException()
        {
            var lon = MIN_LON + PRECISION;
            Assert.Throws<ArgumentException>(() => _punt.Lon = lon);
        }
        #endregion*/
        #endregion
    }
}
