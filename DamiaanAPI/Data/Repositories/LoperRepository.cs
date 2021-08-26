using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Repositories
{
    public class LoperRepository : ILoperRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Loper> _lopers;

        public LoperRepository(ApplicationDbContext context)
        {
            _context = context;
            _lopers = context.Lopers;
        }

        public void Add(Loper loper)
        {
            _lopers.Add(loper);
        }

        public List<Loper> GetAll()
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ToList();
        }

        public Loper GetByEmail(string email)
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(r => r.Route).SingleOrDefault(l => l.Email == email);
        }

        public Loper GetByID(int ID)
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(r => r.Route).SingleOrDefault(l => l.ID == ID);
        }

        public Loper GetByLinkCode(string code)
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(gr => gr.Route).Where(l => l.GeregistreerdeRoutes.Any(rl => rl.LinkCode == code && rl.Route.Start.Date == DateTime.Now.Date && rl.Zichtbaarheid != Zichtbaarheid.Niet && rl.OrderStatus == "9")).Single();

        }

        public Loper GetByOrderId(string code)
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(gr => gr.Route).Where(l => l.GeregistreerdeRoutes.Any(rl => rl.OrderId == code)).Single();

        }

        public Loper GetChat(string code)
        {
            return _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(gr => gr.Messages).Where(l => l.GeregistreerdeRoutes.Any(rl => rl.LinkCode == code)).Single();
        }

        //Zoekt (max 4) lopers die publiek staan en waarvan de wandeling vandaag is.
        //Reden voor new Loper(): email adressen moeten niet worden weergegeven
        public IEnumerable<Loper> SearchPublic(string firstName, string lastName, string gemeente)
        {
            var lopers = (from l in _lopers
                    where (l.FirstName.ToLower().Contains(firstName.ToLower()) || l.LastName.ToLower().Contains(lastName.ToLower()) || l.Gemeente.ToLower().Contains(gemeente.ToLower())) && l.GeregistreerdeRoutes.First(r => r.Route.Start.Date == DateTime.Now.Date && r.Zichtbaarheid == Zichtbaarheid.Openbaar && r.OrderStatus == "9") != null
                    select new Loper { FirstName = l.FirstName, LastName = l.LastName, Gemeente = l.Gemeente, ID = l.ID }).OrderBy(l => l.FirstName).ThenBy(l => l.LastName).Take(4)
                         .ToList();
            lopers.ForEach(l => l.LinkCode = GetLinkCode(l.ID));
            return lopers;
        }

        public Loper SearchHidden(string firstName, string lastName, string gemeente)
        {
            var loper = (from l in _lopers
                         where (l.FirstName.ToLower() == firstName.ToLower() && l.LastName.ToLower() == lastName.ToLower() && l.Gemeente.ToLower() == gemeente.ToLower()) && l.GeregistreerdeRoutes.First(r =>  r.OrderStatus == "9" && r.Route.Start.Date == DateTime.Now.Date && (r.Zichtbaarheid == Zichtbaarheid.Verborgen || r.Zichtbaarheid == Zichtbaarheid.Openbaar)) != null
                         select new Loper { FirstName = l.FirstName, LastName = l.LastName, Gemeente = l.Gemeente, ID = l.ID }).FirstOrDefault();
            loper.LinkCode = GetLinkCode(loper.ID);
            return loper;
        }

        //Reden voor new Loper(): email adressen moeten niet worden weergegeven
        public List<Loper> GetPublicLopers(int page)
        {
            var lopers = (from l in _lopers.Include(l => l.GeregistreerdeRoutes).ThenInclude(rl => rl.Route)
                          where l.GeregistreerdeRoutes.First(rl => rl.Route.Start.Date == DateTime.Now.Date && rl.Zichtbaarheid == Zichtbaarheid.Openbaar && rl.OrderStatus == "9") != null
                          select new Loper { FirstName = l.FirstName, LastName = l.LastName, Gemeente = l.Gemeente, ID = l.ID }).OrderBy(l => l.FirstName).ThenBy(l => l.LastName).Skip(page * 20).Take(20)
                         .ToList();
            lopers.ForEach(l => l.LinkCode = GetLinkCode(l.ID));
            return lopers;
        }


        private string GetLinkCode(int loperId)
        {
            return GetByID(loperId).GeregistreerdeRoutes.Where(rl => rl.Route.Start.Date == DateTime.Now.Date).First().LinkCode;
        }

        public void Update(Loper l)
        {
            _context.Update(l);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
