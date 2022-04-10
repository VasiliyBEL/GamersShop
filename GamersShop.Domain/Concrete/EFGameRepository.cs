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

        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
                context.Games.Add(game);
            else
            {
                Game dbEntry = context.Games.Find(game.GameId);
                if (dbEntry != null)
                {
                    dbEntry.Name = game.Name;
                    dbEntry.Description = game.Description;
                    dbEntry.Price = game.Price;
                    dbEntry.Category = game.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
