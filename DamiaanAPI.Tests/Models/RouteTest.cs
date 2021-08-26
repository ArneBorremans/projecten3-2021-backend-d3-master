using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace DamiaanAPI.Tests.Models
{
    public class RouteTest
    {
        #region TestData
        private readonly Route route;
        private readonly Loper loper;
        private readonly int routeID = 1;
        private Dictionary<string, string> naam { get; set; }
        private readonly double lengte = 1;
        private readonly string geojson = "testgeojson";
        private readonly DateTime start = DateTime.Now.AddDays(1);
        //private readonly string beschrijving = "testbeschrijving";
        private readonly bool openbaar = true;
        private readonly string tshirtMaat = "M";
        private readonly Zichtbaarheid zichtbaarheid = Zichtbaarheid.Openbaar;
        #endregion

        #region Constructor
        public RouteTest()
        {
            naam = new Dictionary<string, string>();
            naam.Add("nl", "test");
            naam.Add("fr", "test frans");
            route = new Route()
            {
                ID = routeID,
                Naam = naam,
                Lengte = lengte,
                GeoJson = geojson,
                Start = start,
                //Beschrijving = beschrijving,
                Openbaar = openbaar
            };

            loper = new Loper();
        }
        #endregion

        #region Test Getters and Setters and Constructor
        [Fact]
        public void testGettersAndSetters_ReturnsCorrectValues()
        {
            Assert.Equal(route.ID, routeID);
            Assert.Equal(route.Naam, naam);
            Assert.Equal(route.Lengte, lengte);
            Assert.Equal(route.GeoJson, geojson);
            Assert.Equal(route.Start, start);
            //Assert.Equal(route.Beschrijving, beschrijving);
            Assert.Equal(route.Openbaar, openbaar);
            Assert.IsType<List<Punt>>(route.Punten);
            Assert.IsType<HashSet<RouteLoper>>(route.Lopers);
        }
        #endregion

        #region Test Methods
        [Fact]
        public void addLoper_ReturnsRouteLoper()
        {
            var res = route.AddLoper(loper, tshirtMaat, zichtbaarheid);

            Assert.IsAssignableFrom<RouteLoper>(res);
        }

        [Fact]
        public void isIngeschreven_ReturnsBool()
        {
            var res = route.IsIngeschreven(loper);

            Assert.IsAssignableFrom<bool>(res);
        }

        [Fact]
        public void removeLoper_NotImplemented_ThrowNotImplementedException()
        {
            Assert.Throws<InvalidOperationException>(() => route.RemoveLoper(loper));
        }
        #endregion

        #region Commented
        /*
        #region Lengte
        [Fact]
        public void Lengte_IsNegatief_GooitException()
        {
            Assert.Throws<ArgumentException>(() =>
            new Route()
            {
                Lengte = 0
            });
        }
        [Fact]
        public void Lengte_IsNul_GooitException()
        {
            Assert.Throws<ArgumentException>(() =>
            new Route()
            {
                Lengte = 0
            }
            );
            
        }

        [Fact]
        public void Lengte_IsPositief_WordtCorrectGeset()
        {
            var lengte = 25;
            _route.Lengte = lengte;
            Assert.Equal(lengte, _route.Lengte);
        }
        #endregion

        #region Start
        [Fact]
        public void Start_IsInHetVerleden_GooitException()
        {
            Assert.Throws<ArgumentException>(() => 
                new Route()
                {
                    Start = DateTime.Now.Subtract(TimeSpan.FromMinutes(1))
                }
            );
        }

        [Fact]
        public void Start_IsInDeToekomst_WordtCorrectGeset()
        {
            var start = DateTime.Today.AddMinutes(1);
            var route = new Route()
            {
                Start = start
            };
            Assert.Equal(start, route.Start);
        }
        #endregion

        #region Lopers
        [Fact]
        public void AddLoper_IsNogNietIngeschreven_WordtToegevoegd()
        {
            _route.AddLoper(_loper, "", Zichtbaarheid.Niet);
            Assert.True(_route.IsIngeschreven(_loper));
        }

        [Fact]
        public void RemoveLoper_LoperIngeschreven_WordtVerwijderd()
        {
            _route.AddLoper(_loper, "", Zichtbaarheid.Niet);
            _route.RemoveLoper(_loper);
            Assert.False(_route.IsIngeschreven(_loper));
        }

        [Fact]
        public void AddLoper_IngeschrevenLoper_WordtGeenTweeKeerIngeschreven()
        {
            var aantalInschrijvingen = _route.Lopers.Count;
            _route.AddLoper(_loper, "", Zichtbaarheid.Niet);
            _route.AddLoper(_loper, "", Zichtbaarheid.Verborgen);
            Assert.Equal(aantalInschrijvingen + 1, _route.Lopers.Count);
        }

        [Fact]
        public void AddLoper_NogNietIngeschreven_RegistratietijdstipWordtCorrectGeset()
        {
            var start = DateTime.Now;
            _route.AddLoper(_loper, "", Zichtbaarheid.Niet);
            var stop = DateTime.Now;
            var inschrijvingstijdstip = _route.Lopers.First(rl => rl.Loper == _loper).GeregistreerdOp;
            Assert.True(inschrijvingstijdstip >= start && inschrijvingstijdstip <= stop);

        }
        #endregion
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n  \t ")]

        public void Naam_IsOngeldig_GooitException(string naam)
        {
            Assert.Throws<ArgumentException>(() => _route.Naam = naam);
        }
        */
        #endregion
    }
}
