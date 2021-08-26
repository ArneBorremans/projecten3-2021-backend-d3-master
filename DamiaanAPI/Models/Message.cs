using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Zender { get; set; }

        public Message() { }
    }
}
