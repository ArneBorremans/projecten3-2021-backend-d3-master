using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class Loper // Of Wandelaar
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gemeente { get; set; }
        public string Email { get; set; }
        public ICollection<RouteLoper> GeregistreerdeRoutes { get; set; }
        //Veld bij RouteLoper, enkel nodig voor respons bij tracking
        [NotMapped]
        public string LinkCode { get; set; }

        public Loper()
        {
            GeregistreerdeRoutes = new HashSet<RouteLoper>();
            //zichtbaarheid = new HashSet<Zichtbaarheid>();
            //Gemeente = "test";
        }

    }
}
