using System;
using Moq;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GamersShop.Domain.Abstract;
using GamersShop.Domain.Entities;
using GamersShop.WebUI.Controllers;
using GamersShop.WebUI.HtmlHelpers;
using GamersShop.WebUI.Models;

namespace GamersShop.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object)
            {
                pageSize = 3
            };

            // Act
            GamesListViewModel result = (GamesListViewModel)controller.List(2).Model;

            // Assert
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Arrange - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Arrange - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Arrange - настройка делегата с помощью лямбда-выражения
            string pageUrlDelegate(int i) => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, (Func<int, string>)pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
    }
}