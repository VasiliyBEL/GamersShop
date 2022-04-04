using GamersShop.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamersShop.WebUI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepository repository;
        public int pageSize = 4;

        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int page = 1)
        {
            return View(repository.Games.OrderBy(game => game.GameId).Skip((page - 1) * pageSize).Take(pageSize));
        }
    }
}