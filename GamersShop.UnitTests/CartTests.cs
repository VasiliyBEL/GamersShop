using GamersShop.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GamersShop.Domain.Entities.Cart;
using System.Collections.Generic;
using System.Linq;
using GamersShop.Domain.Abstract;
using GamersShop.WebUI.Controllers;
using GamersShop.WebUI.Models;
using Moq;
using System.Web.Mvc;

namespace GamersShop.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            // Arrange
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };

            // Arrange
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1" };
            Game game2 = new Game { GameId = 2, Name = "Игра2" };
            Game game3 = new Game { GameId = 3, Name = "Игра3" };

            // Arrange
            Cart cart = new Cart();

            // Arrange
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            // Act
            cart.RemoveLine(game2);

            // Assert
            Assert.AreEqual(cart.Lines.Where(c => c.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

            // Arrange
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            decimal result = cart.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange
            Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

            // Arrange
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Можно добавить игру в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Act
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
            new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Act
            Cart cart = new Cart();

            // Act
            CartController controller = new CartController(mock.Object, null);

            // Arrange
            controller.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Act
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
            new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Act
            Cart cart = new Cart();

            // Act
            CartController controller = new CartController(mock.Object, null);

            // Arrange
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }


        /// <summary>
        /// Проверяем URL
        /// </summary>
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Act
            Cart cart = new Cart();

            // Act
            CartController target = new CartController(null, null);

            // Arrange
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}