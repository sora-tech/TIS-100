using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Boards
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BoardLinkTests
    {
        [Test]
        public void BoardFill_TwoCores_Linked()
        {
            int width = 1;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();

            var top = board.Core(0, 0);
            var bottom = board.Core(0, 1);

            Assert.IsNotNull(top);
            Assert.IsNotNull(bottom);

            Assert.IsTrue(top.Link(Direction.DOWN));
            Assert.IsTrue(bottom.Link(Direction.UP));
        }

        [Test]
        public void BoardFill_TwoCores_NotLinkedEmpty()
        {
            int width = 1;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();

            var top = board.Core(0, 0);
            var bottom = board.Core(0, 1);

            Assert.IsNotNull(top);
            Assert.IsNotNull(bottom);

            Assert.IsFalse(top.Link(Direction.UP));
            Assert.IsFalse(top.Link(Direction.LEFT));
            Assert.IsFalse(top.Link(Direction.RIGHT));

            Assert.IsFalse(bottom.Link(Direction.DOWN));
            Assert.IsFalse(bottom.Link(Direction.LEFT));
            Assert.IsFalse(bottom.Link(Direction.RIGHT));
        }

        [Test]
        public void BoardStep_TwoCores_PassData()
        {
            int width = 1;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();

            var top = board.Core(0, 0);
            var bottom = board.Core(0, 1);

            board.Load(0, 0, "MOV 10 DOWN");
            board.Load(0, 1, "MOV UP ACC");

            Assert.DoesNotThrow(()=> board.Step());

            Assert.AreEqual(10, bottom.ACC);
        }
    }
}

