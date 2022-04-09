using GamersShop.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static GamersShop.Domain.Entities.Cart;
using System.Collections.Generic;
using System.Linq;

namespace GamersShop.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Act
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            // Act
            Cart cart = new Cart();

            // Arrange
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }
    }
}
