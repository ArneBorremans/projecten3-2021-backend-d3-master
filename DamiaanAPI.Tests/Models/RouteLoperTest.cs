using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace DamiaanAPI.Tests.Models
{
    public class RouteLoperTest
    {
        #region TestData
        private readonly RouteLoper routeLoper;
        private readonly Route route;
        private readonly Loper loper;
        private readonly int loperID = 1;
        private readonly int routeID = 1;
        private readonly List<Message> messages = new List<Message>();
        private readonly DateTime geregistreerdOp = DateTime.Now;
        private readonly DateTime startDatumEnUur = DateTime.Now;
        private readonly DateTime eindDatumEnUur = DateTime.Now;
        private readonly decimal laasteLocatieLat = 50;
        private readonly decimal laasteLocatieLon = 50;
        private readonly string tShirtMaat = "XL";
        private readonly Zichtbaarheid zichtbaarheid = Zichtbaarheid.Openbaar;
        private readonly string code = "testCode";
        #endregion

        #region Constructor
        public RouteLoperTest()
        {
            route = new Route();
            loper = new Loper();
            routeLoper = new RouteLoper()
            {
                LoperID = loperID,
                RouteID = routeID,
                Messages = messages,
                GeregistreerdOp = geregistreerdOp,
                StartDatumEnUur = startDatumEnUur,
                EindDatumEnUur = eindDatumEnUur,
                LaatsteLocatieLat = laasteLocatieLat,
                LaatsteLocatieLon = laasteLocatieLon,
                TshirtMaat = tShirtMaat,
                Zichtbaarheid = zichtbaarheid,
                LinkCode = code,
                Loper = loper,
                Route = route
            };
        }
        #endregion

        #region Test Getters and Setters and Constructor
        [Fact]
        public void testGettersAndSetters_ReturnsCorrectValues()
        {
            Assert.Equal(routeLoper.LoperID, loperID);
            Assert.Equal(routeLoper.RouteID, routeID);
            Assert.Equal(routeLoper.Messages, messages);
            Assert.Equal(routeLoper.GeregistreerdOp, geregistreerdOp);
            Assert.Equal(routeLoper.StartDatumEnUur, startDatumEnUur);
            Assert.Equal(routeLoper.EindDatumEnUur, eindDatumEnUur);
            Assert.Equal(routeLoper.LaatsteLocatieLat, laasteLocatieLat);
            Assert.Equal(routeLoper.LaatsteLocatieLon, laasteLocatieLon);
            Assert.Equal(routeLoper.TshirtMaat, tShirtMaat);
            Assert.Equal(routeLoper.Zichtbaarheid, zichtbaarheid);
            Assert.Equal(routeLoper.LinkCode, code);
            Assert.Equal(routeLoper.Loper, loper);
            Assert.Equal(routeLoper.Route, route);
        }
        #endregion
    }
}
