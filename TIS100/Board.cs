using System.Collections.Generic;
using TIS100.Types;

/* The Board
 * 
 * This is the top level component of the emulator and the effective
 * public API.  It specifies a grid size and if each spot in it contains
 * a core.  At the edge of the grid Input and Output queues provide entry
 * and exit points for data.
 * 
 * Buffers the values of all Inputs and Cores before Executing an instruction
 * on every Core and writing to Outputs.
 */
namespace TIS100
{
    public class Board
    {
        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;

            cores = new Core[this.width, this.height];

            inputs = new Dictionary<Position, Link>();
            outputs = new Dictionary<Position, Link>();
        }

        private readonly int width;
        private readonly int height;

        // The grid
        private readonly Core[,] cores;

        private readonly Dictionary<Position, Link> inputs;
        private readonly Dictionary<Position, Link> outputs;

        public static List<Direction> AllDirections = new List<Direction>() { Direction.UP, Direction.RIGHT, Direction.DOWN, Direction.LEFT };

        public int Size { get { return this.width * this.height; } }

        // Calculate the offset from a grid position based on the provided direction
        private Position Move(int w, int h, Direction dir)
        {
            return dir switch
            {
                Direction.UP => new Position(w, h - 1),
                Direction.RIGHT => new Position(w + 1, h),
                Direction.DOWN => new Position(w, h + 1),
                Direction.LEFT => new Position(w - 1, h),
                _ => new Position(w, h),
            };
        }

        // Gives the reverse of a direction: a Right link on one node is a Left on the other
        private Direction Invert(Direction dir)
        {
            return dir switch
            {
                Direction.UP => Direction.DOWN,
                Direction.RIGHT => Direction.LEFT,
                Direction.DOWN => Direction.UP,
                Direction.LEFT => Direction.RIGHT,
                _ => Direction.UP,
            };
        }

        private void AddLink(int w, int h, Direction dir)
        {
            var pos = Move(w, h, dir);
            Core(pos.width, pos.height)?.AddDirection(Invert(dir));
        }

        // Attempt to read out the inputs and set the linked core input value
        private void Buffer()
        {
            foreach (var input in inputs)
            {
                if(input.Value.values.TryDequeue(out var value) == false)
                {
                    continue;
                }

                var pos = Move(input.Key.width, input.Key.height, input.Value.direction);
                if (Core(pos.width, pos.height)?.SetDirection(Invert(input.Value.direction), value) == false)
                {
                    input.Value.values.Enqueue(value);
                }
            }
        }

        // Execute a full step of every input, output and core on the board
        public void Step()
        {
            Buffer();

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var core = Core(w, h);
                    if(core == null)
                    {
                        continue;
                    }

                    core.Step();

                    foreach (var dir in AllDirections)
                    {
                        if (core.Link(dir) && core.GetDirection(dir, out var result))
                        {
                            var linkPos = Move(w, h, dir);

                            Core(linkPos.width, linkPos.height)?.SetDirection(Invert(dir), result);

                            if (outputs.ContainsKey(linkPos))
                            {
                                outputs[linkPos].values.Enqueue(result);
                            }
                        }
                    }
                }
            }
        }

        // Add or replace an input on the board with the provided values
        public Position AddInput(int w, int h, Direction dir, Queue<int> values)
        {
            var pos = new Position(w, h);
            AddLink(w, h, dir);

            if (inputs.ContainsKey(pos))
            {
                var id = inputs[pos].id;
                var link = new Link(id, dir, values);
                inputs[pos] = link;
            }
            else
            {
                var link = new Link(inputs.Count, dir, values);
                inputs.Add(pos, link);
            }

            return pos;
        }

        public Link GetInput(Position pos)
        {
            return inputs[pos];
        }

        public Position AddOutput(int w, int h, Direction dir)
        {
            var output = new Link(outputs.Count, dir);
            var pos = new Position(w, h);
            outputs.Add(pos, output);

            AddLink(w, h, dir);
            
            return pos;
        }

        public Link GetOutput(Position pos)
        {
            return outputs[pos];
        }

        public Core Core(int w, int h)
        {
            if(w < 0 || h < 0)
            {
                return null;
            }
            if (w >= this.width || h >= this.height)
            {
                return null;
            }

            return cores[w, h];
        }

        // Create a core on the grid and automatically link it to its neighbours
        public void Place(int w, int h)
        {
            cores[w, h] = new Core();

            var directions = new List<Direction>();

            foreach (var dir in AllDirections)
            {
                var pos = Move(w, h, dir);
                if (Core(pos.width, pos.height) != null)
                {
                    cores[pos.width, pos.height].AddDirection(Invert(dir));
                    directions.Add(dir);
                }
            }

            cores[w, h].SetDirections(directions);
        }

        // Turn the provided code into instructions for a core to run
        public List<Error> Load(int w, int h, string code)
        {
            var commands = Parser.Instruct(Lexer.Lex(code));
            cores[w, h]?.SetCommands(commands);

            return cores[w, h]?.Validate();
        }

        // Shortcut method to fill the entire grid with cores
        public void Fill()
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    Place(w, h);
                }
            }
        }
    }
}
