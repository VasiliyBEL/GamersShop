using GamersShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamersShop.Domain.Abstract
{
    public interface IGameRepository
    {
        IEnumerable<Game> Games { get; }
        void SaveGame(Game game);
    }
}