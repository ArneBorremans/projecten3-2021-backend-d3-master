using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DamiaanAPI.Tests.MockData
{
    public class MockRouteLoperRepositoryData
    {
        #region Properties

        #region Loper
        public Loper loper { get; set; }
        public Loper loperMetGeregistreerdeRoutes { get; set; }
        #endregion

        #region Loper List
        public IEnumerable<Loper> lopers { get; set; }
        #endregion

        #region Message
        public MessageDTO messageDTO { get; set; }
        #endregion

        #region RouteLoper
        public Dictionary<string, string> naam { get; set; }

        public RouteLoper routeLoper { get; set; }
        #endregion

        #region RouteLoperDTO
        public RouteLoperDTO routeLoperDTO { get; set; }
        #endregion

        #region HuidigeLocatieDTO
        public HuidigeLocatieDTO huidigeLocatieDTOFive { get; set; }

        public HuidigeLocatieDTO huidigeLocatieDTOOne { get; set; }
        #endregion

        #endregion

        #region Constructor
        public MockRouteLoperRepositoryData()
        {
            // INITIALIZE DATA
            #region Loper
            naam = new Dictionary<string, string>();
            naam.Add("nl", "test");
            naam.Add("fr", "test frans");
            loper = new Loper()
            {
                ID = 1,
                FirstName = "John",
                LastName = "Parsley",
                Email = "John@Parsley.com",
                Gemeente = "Gent"
            };
            loperMetGeregistreerdeRoutes = new Loper()
            {
                ID = 1,
                LinkCode = "testing",
                FirstName = "John",
                LastName = "Parsley",
                Email = "John@Parsley.com",
                Gemeente = "Gent",
                GeregistreerdeRoutes = new List<RouteLoper>() {
                    new RouteLoper() {
                        RouteID = 1,
                        Route = new Route()
                        {
                            Naam = naam,
                            ID = 1,
                            Start = DateTime.Now.AddDays(1)
                        },
                        Messages = new List<Message>()
                        {
                            new Message() { }
                        },
                        LinkCode = "testing"
                    }
                }
            };
            #endregion

            #region Loper List
            lopers = new List<Loper> { loper, loper };
            #endregion

            #region Message
            messageDTO = new MessageDTO()
            { Date = DateTime.Now, Text = "testing", Zender = "Jacob" };
            #endregion

            #region RouteLoper
            routeLoper = new RouteLoper()
            {
                RouteID = 1,
                Messages = new List<Message>()
                {
                        new Message() { }
                },
                LinkCode = "testing"
            };
            #endregion

            #region RouteLoperDTO
            routeLoperDTO = new RouteLoperDTO
            {
                LoperID = 1,
                RouteID = 1
            };
            #endregion

            #region HuidigeLocatieDTO
            huidigeLocatieDTOFive = new HuidigeLocatieDTO()
            {
                RouteId = 5
            };

            huidigeLocatieDTOOne = new HuidigeLocatieDTO()
            {
                RouteId = 1
            };
            #endregion
            // END
        }
        #endregion
    }
}