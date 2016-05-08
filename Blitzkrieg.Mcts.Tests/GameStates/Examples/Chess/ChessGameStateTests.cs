using Blitzkrieg.Mcts.GameStates.Examples.Chess;
using NUnit.Framework;

namespace Blitzkrieg.Mcts.Tests.GameStates.Examples.Chess
{
    [TestFixture]
    public class ChessGameStateTests
    {
        public ChessGameState GameState;

        [SetUp]
        public void Init()
        {
            GameState = new ChessGameState();
        }

        [Test]
        public void Test()
        {
            var a = new ChessGameState();
            Assert.That(a, Is.Not.Null);
        }
    }
}
