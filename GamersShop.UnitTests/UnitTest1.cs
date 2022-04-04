using GamersShop.Domain.Abstract;
using GamersShop.Domain.Entities;
using GamersShop.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

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
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"}
            });
            GameController controller = new GameController(mock.Object)
            {
                pageSize = 3
            };

            // Act
            IEnumerable<Game> result = (IEnumerable<Game>)controller.List(2).Model;

            // Assert
            List<Game> games = result.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Game4");
            Assert.AreEqual(games[1].Name, "Game5");
        }
    }
}