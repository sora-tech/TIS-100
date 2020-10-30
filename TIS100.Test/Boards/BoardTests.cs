using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace TIS100.Test.Boards
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BoardTests
    {
        [Test]
        public void BoardConstructor_Values_HasSize()
        {
            var board = new Board(2, 2);

            Assert.AreEqual(4, board.Size);
        }

        [Test]
        public void BoardConstructor_Core_Empty()
        {
            var board = new Board(2, 2);

            var core = board.Core(1, 1);

            Assert.IsNull(core);
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void Board_CoreUnderSize_Empty(int w, int h)
        {
            var board = new Board(2, 2);

            var core = board.Core(w, h);

            Assert.IsNull(core);
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void Board_CoreOverSize_Empty(int w, int h)
        {
            var board = new Board(2, 2);

            var core = board.Core(1 + w, 1 + h);

            Assert.IsNull(core);
        }

        [Test]
        public void BoardSetCore_Core_HasCore()
        {
            var board = new Board(2, 2);

            board.Place(1, 1);
            var core = board.Core(1, 1);

            Assert.NotNull(core);
        }

        [Test]
        public void BoardFill_AllCore_HasCore()
        {
            int width = 2;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();
            
            for(int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var core = board.Core(w, h);
                    Assert.NotNull(core);
                }
            }
        }

        [Test]
        public void BoardLoad_ValidCode_Loads()
        {
            var board = new Board(1, 1);
            board.Fill();

            var errors = board.Load(0, 0, "ADD 1");

            Assert.IsEmpty(errors);
        }

        [Test]
        public void BoardLoad_InValidCode_HasErrors()
        {
            var board = new Board(1, 1);
            board.Fill();

            var errors = board.Load(0, 0, "ADD ADD");

            Assert.IsNotEmpty(errors);
        }
    }
}

