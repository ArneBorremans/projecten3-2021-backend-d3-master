using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public interface IRouteRepository
    {
        public void NewRoute(Route r);
        public Route GetByIdGeoJson(int id);
        public Route GetById(int id);
        public void Update(Route r);
        public void Delete(Route r);
        public IEnumerable<Route> GetAll();
        public IEnumerable<Route> GetPublicRoutes();
        void SaveChanges();
    }
}
