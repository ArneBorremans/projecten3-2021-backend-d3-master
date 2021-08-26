using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public interface IPuntRepository
    {
        public void NewPunt(Punt p);
        public Punt GetById(int id);
        public void Update(Punt p);
        public void Delete(Punt p);
        void SaveChanges();
    }
}
