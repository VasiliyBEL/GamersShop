using GamersShop.Domain.Abstract;
using GamersShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamersShop.Domain.Concrete
{
    public class EFGameRepository : IGameRepository
    {
        readonly EFDbContext context = new EFDbContext();

        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }
    }
}
