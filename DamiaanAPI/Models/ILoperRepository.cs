using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Models
{
    public interface ILoperRepository
    {
        List<Loper> GetAll();
        IEnumerable<Loper> SearchPublic(string firstName, string lastName, string gemeente);
        Loper SearchHidden(string firstName, string lastName, string gemeente);
        Loper GetByEmail(string email);
        void Add(Loper loper);
        void SaveChanges();
        List<Loper> GetPublicLopers(int page);
        Loper GetByLinkCode(string code);
        Loper GetByOrderId(string code);
        Loper GetByID(int loperID);
        Loper GetChat(string code);
        void Update(Loper l);
    }
}
