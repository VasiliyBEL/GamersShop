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
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

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

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
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
            GamesListViewModel result
                = (GamesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                 {
                       new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
                       new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
                       new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
                       new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
                       new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
                 });
            GameController controller = new GameController(mock.Object)
            {
                pageSize = 3
            };

            // Action
            List<Game> result = ((GamesListViewModel)controller.List("Cat2", 1).Model)
                .Games.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
            new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
            new Game { GameId = 2, Name = "Игра2", Category="Симулятор"},
            new Game { GameId = 3, Name = "Игра3", Category="Шутер"},
            new Game { GameId = 4, Name = "Игра4", Category="RPG"},
            });

            // Arrange
            NavController target = new NavController(mock.Object);

            // Act
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Assert
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "RPG");
            Assert.AreEqual(results[1], "Симулятор");
            Assert.AreEqual(results[2], "Шутер");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[] {
                 new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
                 new Game { GameId = 2, Name = "Игра2", Category="Шутер"}
             });

            // Arrange
            NavController target = new NavController(mock.Object);

            // Arrange
            string categoryToSelect = "Шутер";

            // Act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                 new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
                 new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
                 new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
                 new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
                 new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
            });
            GameController controller = new GameController(mock.Object)
            {
                pageSize = 3
            };

            // Act
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}