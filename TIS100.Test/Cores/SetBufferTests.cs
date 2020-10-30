using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SetBufferTests
    {
        [Test]
        public void Set_TwiceWithoutRead_Fails()
        {
            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.DOWN });

            core.SetDirection(Direction.DOWN, 5);
            var set = core.SetDirection(Direction.DOWN, 5);

            Assert.IsFalse(set);
        }
    }
}
