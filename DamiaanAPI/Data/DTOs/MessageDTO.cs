using System;
using DamiaanAPI.Models;

namespace DamiaanAPI.Data.DTOs
{
    public class MessageDTO
    {

        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Zender { get; set; }

        public MessageDTO(Message m)
        {
            this.Text = m.Text;
            this.Date = m.Date;
            this.Zender = m.Zender;
        }

        public MessageDTO() { }
    }
}
