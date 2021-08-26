using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Route> _routes;

        public RouteRepository(ApplicationDbContext context)
        {
            _context = context;
            _routes = context.Routes;
        }

        public void NewRoute(Route r)
        {
            _routes.Add(r);
            SaveChanges();
        }

        public Route GetByIdGeoJson(int id)
        {
            return _routes.Where(r => r.ID == id).Include(r => r.Punten).Include(r => r.Lopers).FirstOrDefault();
        }

        public Route GetById(int id)
        {
            return _routes.Where(r => r.ID == id).Include(r => r.Punten).FirstOrDefault();
        }

        public IEnumerable<Route> GetAll()
        {
            return _routes;
        }

        public IEnumerable<Route> GetPublicRoutes()
        {
            return GetAll().Where(r => r.Openbaar == true);
        }

        public void Delete(Route r)
        {
            _context.Remove(r);
            SaveChanges();
        }

        public void Update(Route r)
        {
            _context.Update(r);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
