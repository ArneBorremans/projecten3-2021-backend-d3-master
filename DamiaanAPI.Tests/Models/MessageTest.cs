using DamiaanAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DamiaanAPI.Tests.Models
{
    public class MessageTest
    {
        #region TestData
        private Message message;
        private readonly int messageID = 1;
        private readonly string text = "test";
        private readonly DateTime date = DateTime.Now;
        private readonly string zender = "tester";
        #endregion

        #region Constructor
        public MessageTest()
        {
            message = new Message()
            {
                ID = messageID,
                Text = text,
                Date = date,
                Zender = zender
            };
        }
        #endregion

        #region Test Getters and Setters
        [Fact]
        public void testGettersAndSetters_ReturnsCorrectValues()
        {
            Assert.Equal(message.ID, messageID);
            Assert.Equal(message.Text, text);
            Assert.Equal(message.Date, date);
            Assert.Equal(message.Zender, zender);
        }
        #endregion
    }
}
