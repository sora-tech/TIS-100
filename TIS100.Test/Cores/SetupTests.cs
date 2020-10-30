using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SetupTests
    {
        [Test]
        public void Setup_Defaults_AccZero()
        {
            var core = new Core();

            Assert.AreEqual(0, core.ACC);
        }

        [Test]
        public void Setup_Defaults_BakZero()
        {
            var core = new Core();

            Assert.AreEqual(0, core.BAK);
        }
        [Test]
        public void Setup_Defaults_ProgramCounterZero()
        {
            var core = new Core();

            Assert.AreEqual(0, core.PC);
        }

        [Test]
        [TestCase(Direction.UP)]
        [TestCase(Direction.RIGHT)]
        [TestCase(Direction.DOWN)]
        [TestCase(Direction.LEFT)]
        public void Setup_Defaults_NoDirections(Direction direction)
        {
            var core = new Core();

            Assert.IsFalse(core.Link(direction));
        }

        [Test]
        [TestCase(Direction.UP)]
        [TestCase(Direction.RIGHT)]
        [TestCase(Direction.DOWN)]
        [TestCase(Direction.LEFT)]
        public void Setup_WithDirection_HasDirection(Direction direction)
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { direction });

            Assert.IsTrue(core.Link(direction));
        }


        [Test]
        [TestCase(Direction.UP)]
        [TestCase(Direction.RIGHT)]
        [TestCase(Direction.DOWN)]
        [TestCase(Direction.LEFT)]
        public void Setup_WithDirection_HasOnlyDirection(Direction link)
        {
            var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            directions.Remove(link);

            var core = new Core();
            core.SetDirections(new List<Direction> { link });

            Assert.IsTrue(core.Link(link));

            foreach (var d in directions)
            {
                Assert.IsFalse(core.Link(d));
            }
        }

        [Test]
        [TestCase(Direction.UP)]
        [TestCase(Direction.RIGHT)]
        [TestCase(Direction.DOWN)]
        [TestCase(Direction.LEFT)]
        public void Setup_AddDirection_HasDirections(Direction direction)
        {
            var core = new Core();

            core.AddDirection(direction);

            Assert.IsTrue(core.Link(direction));
        }

        [Test]
        [TestCase(Direction.UP)]
        [TestCase(Direction.RIGHT)]
        [TestCase(Direction.DOWN)]
        [TestCase(Direction.LEFT)]
        public void Setup_AddDirectionTwice_DoesNotThrow(Direction direction)
        {
            var core = new Core();

            core.AddDirection(direction);

            Assert.DoesNotThrow(() => core.AddDirection(direction));

            Assert.IsTrue(core.Link(direction));
        }
    }
}
