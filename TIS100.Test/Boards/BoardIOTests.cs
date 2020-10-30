using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Boards
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BoardIOTests
    {
        [Test]
        [TestCase(0, -1, Direction.DOWN, Direction.UP)]
        [TestCase(0, 1, Direction.UP, Direction.DOWN)]
        [TestCase(-1, 0, Direction.RIGHT, Direction.LEFT)]
        [TestCase(1, 0, Direction.LEFT, Direction.RIGHT)]
        public void BoardInput_OneCores_Linked(int w, int h, Direction dir, Direction result)
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            board.AddInput(w, h, dir, new Queue<int>());

            var core = board.Core(0, 0);

            Assert.IsTrue(core.Link(result));
        }

        [Test]
        public void BoardInput_FourCores_LinkedEdgeOnly()
        {
            int width = 2;
            int height = 2;
            var board = new Board(width, height);

            board.Fill();

            board.AddInput(0, -1, Direction.DOWN, new Queue<int>());

            var core = board.Core(0, 0);

            Assert.IsTrue(core.Link(Direction.UP));
            Assert.IsTrue(core.Link(Direction.DOWN));
            Assert.IsTrue(core.Link(Direction.RIGHT));
            Assert.IsFalse(core.Link(Direction.LEFT));
        }

        [Test]
        [TestCase(0, -1, Direction.DOWN, Direction.UP)]
        [TestCase(0, 1, Direction.UP, Direction.DOWN)]
        [TestCase(-1, 0, Direction.RIGHT, Direction.LEFT)]
        [TestCase(1, 0, Direction.LEFT, Direction.RIGHT)]
        public void BoardOutput_OneCore_Linked(int w, int h, Direction dir, Direction result)
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            board.AddOutput(w, h, dir);

            var core = board.Core(0, 0);

            Assert.IsTrue(core.Link(result));
        }

        [Test]
        public void BoardInput_InputValues_Saved()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            var pos = board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 1, 2, 3 }));

            var link = board.GetInput(pos);
            Assert.AreEqual(0, pos.width);
            Assert.AreEqual(-1, pos.height);

            Assert.IsNotNull(link);
            Assert.AreEqual(0, link.id);
            Assert.AreEqual(Direction.DOWN, link.direction);
            Assert.AreEqual(3, link.values.Count);
        }

        [Test]
        public void BoardInput_InputValuesTwice_Overwrites()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 1, 2, 3 }));
            var pos = board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 4, 3, 2, 1 }));

            var link = board.GetInput(pos);

            Assert.AreEqual(0, pos.width);
            Assert.AreEqual(-1, pos.height);

            Assert.IsNotNull(link);
            Assert.AreEqual(0, link.id);
            Assert.AreEqual(Direction.DOWN, link.direction);
            Assert.AreEqual(4, link.values.Count);
            Assert.AreEqual(4, link.values.Peek());
        }

        [Test]
        public void BoardInput_InputBufferWithoutRead_Leaves()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            var pos = board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 1, 3, 5 }));

            var input = board.GetInput(pos);

            Assert.DoesNotThrow(() => board.Step());

            Assert.DoesNotThrow(() => board.Step());

            Assert.AreEqual(2, input.values.Count);
        }

        [Test]
        public void BoardInput_InputValues_Read()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();
            board.Load(0, 0, "MOV UP ACC");

            board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 1, 3, 5 }));

            var core = board.Core(0, 0);

            board.Step();
            Assert.AreEqual(1, core.ACC);
            board.Step();
            Assert.AreEqual(3, core.ACC);
            board.Step();
            Assert.AreEqual(5, core.ACC);
        }

        [Test]
        public void BoardInput_InputEmpty_ReadWithoutError()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();
            board.Load(0, 0, "MOV UP ACC");

            board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 1 }));

            var core = board.Core(0, 0);

            board.Step();
            Assert.AreEqual(1, core.ACC);

            Assert.DoesNotThrow(() => board.Step());
        }

        [Test]
        public void BoardOutput_AddOutput_HasId()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            board.AddOutput(0, -1, Direction.DOWN);
            var pos = board.AddOutput(0, 1, Direction.UP);

            var link = board.GetOutput(pos);
            Assert.AreEqual(0, pos.width);
            Assert.AreEqual(1, pos.height);

            Assert.AreEqual(1, link.id);
        }

        [Test]
        public void BoardOutput_AddOutput_Empty()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();

            var pos = board.AddOutput(0, 1, Direction.UP);

            var link = board.GetOutput(pos);

            Assert.IsEmpty(link.values);
        }

        [Test]
        public void BoardOutput_OutputValues_Writen()
        {
            int width = 1;
            int height = 1;
            var board = new Board(width, height);

            board.Fill();
            board.Load(0, 0, @"MOV 5 DOWN
MOV 10 DOWN");

            var pos = board.AddOutput(0, 1, Direction.UP);

            board.Step();
            board.Step();

            var link = board.GetOutput(pos);
            Assert.IsNotEmpty(link.values);
            Assert.AreEqual(5, link.values.Dequeue());
            Assert.AreEqual(10, link.values.Dequeue());
        }
    }
}


