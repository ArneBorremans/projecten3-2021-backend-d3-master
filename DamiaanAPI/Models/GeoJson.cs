using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public class GeoJson
    {
        public string Type { get; set; }
        public object Properties { get; set; }
        public GeoJsonGeometry Geometry { get; set; }

        public class GeoJsonGeometry
        {
            public string Type { get; set; }
            public IList<IList<decimal>> Coordinates { get; set; }
        }
    }
}
