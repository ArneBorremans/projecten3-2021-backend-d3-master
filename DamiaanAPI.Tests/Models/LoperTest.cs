using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DamiaanAPI.Tests.Models
{
    public class LoperTest
    {
        #region TestData
        private Loper loper;
        private readonly string email = "test@test.be";
        private readonly string firstName = "test";
        private readonly string lastName = "tester";
        private readonly string gemeente = "testGemeente";
        private readonly string code = "testCode";
        private readonly int loperID = 1;
        #endregion

        #region Constructor
        public LoperTest()
        {
            loper = new Loper()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Gemeente = gemeente,
                ID = loperID,
                LinkCode = code
            };
        }
        #endregion

        #region Test Getters and Setters and Constructor
        [Fact]
        public void testGettersAndSetters_ReturnsCorrectValues()
        {
            Assert.Equal(loper.Email, email);
            Assert.Equal(loper.FirstName, firstName);
            Assert.Equal(loper.LastName, lastName);
            Assert.Equal(loper.Gemeente, gemeente);
            Assert.Equal(loper.ID, loperID);
            Assert.Equal(loper.LinkCode, code);
            Assert.IsType<HashSet<RouteLoper>>(loper.GeregistreerdeRoutes);
        }
        #endregion
    }
}
