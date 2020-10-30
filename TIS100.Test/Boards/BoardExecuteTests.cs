using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace TIS100.Test.Boards
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BoardRunOnceTests
    {
        [Test]
        public void BoardStep_Empty_DoesNotThrow()
        {
            int width = 0;
            int height = 0;
            var board = new Board(width, height);

            board.Fill();

            Assert.DoesNotThrow(() => board.Step());
        }

        [Test]
        public void BoardStep_Single_DoesNotThrow()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            Assert.DoesNotThrow(() => board.Step());
        }

        [Test]
        public void BoardStep_TwoCores_RunOnces()
        {
            int width = 1;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();

            board.Load(0, 0, "ADD 2");
            board.Load(0, 1, "ADD 4");

            board.Step();

            var acc1 = board.Core(0, 0).ACC;
            var acc2 = board.Core(0, 1).ACC;

            Assert.AreEqual(2, acc1);
            Assert.AreEqual(4, acc2);
        }

    }
}

