using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class RouteLoper
    {
        #region Properties
        public int RouteID { get; set; }
        public Route Route { get; set; }
        public int LoperID { get; set; }
        public Loper Loper { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime GeregistreerdOp { get; set; }
        public DateTime? StartDatumEnUur { get; set; }
        public DateTime? EindDatumEnUur { get; set; }
        public decimal? LaatsteLocatieLat { get; set; }
        public decimal? LaatsteLocatieLon { get; set; }
        public string TshirtMaat { get; set; }
        public Zichtbaarheid Zichtbaarheid { get; set; }
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string LinkCode { get; set; }
        #endregion

        #region Constructor & methods
        public RouteLoper(Route route, Loper loper)
        {
            Route = route;
            RouteID = route.ID;
            Loper = loper;
            LoperID = loper.ID;
            GeregistreerdOp = DateTime.Now;

            MD5 md5Hash = MD5.Create();
            LinkCode = GetMd5Hash(md5Hash, DateTime.Now.ToString("ddMMyyyymmHHssFF"));

            Messages = new List<Message>();
        }

        public RouteLoper()
        {
           
        }

        public override bool Equals(object obj)
        {
            if(obj is RouteLoper)
            {
                var other = (RouteLoper)obj;
                return RouteID == other.RouteID && LoperID == other.LoperID;
            }
            return false; 
        }

        public override int GetHashCode()
        {
            return RouteID.GetHashCode() + LoperID.GetHashCode();
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion


    }
}
