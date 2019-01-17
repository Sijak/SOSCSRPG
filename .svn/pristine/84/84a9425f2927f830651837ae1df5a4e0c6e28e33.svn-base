using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine.ViewModels;

namespace TestEngine.ViewModels
{
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestGameSession1()
        {
            GameSession gameSession = new GameSession();
            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual(gameSession.CurrentLocation.Name, "Home");
        }
        [TestMethod]
        public void TestPlayerIsMovedToHomeAndCompletelyHealedWhenKilled()
        {
            GameSession game = new GameSession();
            game.CurrentPlayer.TakeDamage(999);
            Assert.AreEqual(game.CurrentLocation.Name, "Home");
            Assert.AreEqual(game.CurrentPlayer.Level*10, game.CurrentPlayer.CurrentHitPoint);
        }

    }
}
