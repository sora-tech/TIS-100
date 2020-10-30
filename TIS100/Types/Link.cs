using System.Collections.Generic;

/* Links are the Input and Output for a board */

namespace TIS100.Types
{
    public readonly struct Link
    {
        public readonly int id;
        public readonly Direction direction;
        public readonly Queue<int> values;

        public Link(int id, Direction direction) : this(id, direction, new Queue<int>()) { }
        public Link(int id, Direction direction, Queue<int> values)
        {
            this.id = id;
            this.direction = direction;
            this.values = values;
        }
    }
}
