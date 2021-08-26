using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace DamiaanAPI.Tests
{
    public class PuntTest
    {
        private readonly Punt _punt;

        private static readonly decimal MIN_LAT = -90;
        private static readonly decimal MAX_LAT = 90;
        private static readonly decimal MIN_LON = -180;
        private static readonly decimal MAX_LON = 180;
        private static readonly decimal PRECISION = 0.000000000000001M;

        public PuntTest()
        {
            _punt = new Punt();
        }

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
            var lon = MAX_LON + PRECISION;
            Assert.Throws<ArgumentException>(() => _punt.Lon = lon);
        }
        #endregion

        #region Faciliteiten
        [Fact]
        public void VoegFaciliteitToe_NieuweFaciliteit_WordtToegevoegd()
        {
            var faciliteit = "test";
            var aantalFaciliteiten = _punt.Faciliteiten.Count();
            _punt.VoegFaciliteitToe(faciliteit);
            Assert.Equal(aantalFaciliteiten + 1, _punt.Faciliteiten.Count());
        }

        [Fact]
        public void VoegFaciliteitToe_FaciliteitAlToegevoegd_WordtNietOpnieuwToegevoegd()
        {
            var faciliteit = "test";
            var aantalFaciliteiten = _punt.Faciliteiten.Count();
            _punt.VoegFaciliteitToe(faciliteit);
            _punt.VoegFaciliteitToe(faciliteit);
            Assert.Equal(aantalFaciliteiten + 1, _punt.Faciliteiten.Count());
        }
        #endregion
    }
}
