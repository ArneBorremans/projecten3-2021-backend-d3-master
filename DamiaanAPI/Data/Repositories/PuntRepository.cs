using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DamiaanAPI.Data.Repositories
{
    public class PuntRepository : IPuntRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<Punt> _punten;

        public PuntRepository(ApplicationDbContext context)
        {
            _context = context;
            _punten = context.Punten;
        }

        public void Delete(Punt p)
        {
            _context.Remove(p);
            SaveChanges();
        }

        public Punt GetById(int id)
        {
            return _punten.SingleOrDefault(p => p.ID == id);
        }

        public void NewPunt(Punt p)
        {
            _punten.Add(p);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Punt p)
        {
            _context.Update(p);
            SaveChanges();
        }
    }
}
