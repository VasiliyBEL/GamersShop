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
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
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

            // Arrange
            AdminController controller = new AdminController(mock.Object);

            // Act
            List<Game> result = ((IEnumerable<Game>)controller.Index().
                ViewData.Model).ToList();

            // Assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Игра1", result[0].Name);
            Assert.AreEqual("Игра2", result[1].Name);
            Assert.AreEqual("Игра3", result[2].Name);
        }
    }
}