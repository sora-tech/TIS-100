using System;
using System.Collections.Generic;
using System.Linq;
using TIS100.Types;

/* The Core
 * 
 * This contains the execution of the code produced by the Lexer & Parser.
 * It can run as a standalone unit but is expected to be hosted on a Board.
 * 
 * Can be linked in any Direction and pauses execution on reading and writing
 * those until cleared.
 */

namespace TIS100
{
    public class Core
    {
        public Core()
        {
            ACC = 0;
            BAK = 0;
            PC = 0;

            input = new Dictionary<Direction, int>();
            output = new Dictionary<Direction, int>();
            directions = new HashSet<Direction>();
            commands = new List<Command>();
        }

        readonly Dictionary<Direction, int> input;
        readonly Dictionary<Direction, int> output;

        private HashSet<Direction> directions;
        private List<Command> commands;

        // Accumulator: the working memory of each core
        public int ACC { get; private set; }

        // Backup: saved memory from the accumulator
        public int BAK { get; private set; }

        // Program Counter: sets the next instruction to be executed
        public int PC { get; private set; }

        public bool Link(Direction direction)
        {
            return directions.Contains(direction);
        }

        public List<Error> Validate()
        {
            return Validator.Validate(this.directions, this.commands);
        }

        public void SetDirections(List<Direction> links)
        {
            directions = new HashSet<Direction>(links);
        }

        public void SetCommands(List<Command> commands)
        {
            this.commands = commands;
        }

        // Step through the code once for testing
        // Normal execution loops forever
        // This can infinite loop (by design)
        internal void RunOnce()
        {
            if (this.commands.Count == 0)
            {
                return;
            }

            while (this.PC < this.commands.Count)
            {
                Step();
            }
        }

        // Read a number value from any of the possible locations
        private bool TryReadNumber(Parameter token, out int value)
        {
            value = 0;
            if (token.param == Param.Number)
            {
                value = token.number;
                return true;
            }
            
            if (token.param == Param.ACC)
            {
                value = this.ACC;
                return true;
            }

            if(token.value == "NIL")
            {
                return true;
            }

            if (token.param == Param.Direction)
            {
                // If no value has been buffered the output is 0
                // and return false
                return ReadDirection(token.direction, out value);
            }

            return false;
        }

        // Attempt to read a value from a link
        // If no value is present the execution repeats until
        // one is available
        private bool ReadDirection(Direction dir, out int value)
        {
            if(input.TryGetValue(dir, out value))
            {
                input.Remove(dir);
                return true;
            }
            return false;
        }

        public void AddDirection(Direction direction)
        {
            this.directions.Add(direction);
        }

        private void BufferDirection(Direction dir, int value)
        {
            output[dir] = value;
        }

        // Read the output buffer of a link
        public bool GetDirection(Direction dir, out int result)
        {
            result = 0;
            if (output.ContainsKey(dir) == false)
            {
                return false;
            }

            result = output[dir];
            output.Remove(dir);

            return true;
        }

        // Attempt to write the input buffer of a link
        public bool SetDirection(Direction dir, int value)
        {
            if (input.ContainsKey(dir))
            {
                return false;
            }

            input[dir] = value;

            return true;
        }

        private int JumpOffset(Func<bool> jumpCondition, string label)
        {
            if (jumpCondition())
            {
                //Find the target label and set the offset to advance to it
                var jmpLabel = this.commands.First(c => c.instruction == Instruction.LBL && c.primary.value == label);
                return jmpLabel.line - this.PC;
            }
            // Advance to the next instruction
            return 1;
        }

        // Execute the current instruction at PC
        // and advance PC or loop
        public void Step()
        {
            if (this.PC >= this.commands.Count)
            {
                this.PC = 0;
            }

            if (this.output.Count != 0)
            {
                return;
            }

            if(this.commands.Count == 0)
            {
                return;
            }

            var command = this.commands[this.PC];
            var offset = 1; // Hold the value to be applied to the PC

            switch (command.instruction)
            {
                case Instruction.LBL:
                case Instruction.NOP:
                    // do nothing
                    break;
                case Instruction.MOV:
                    if (TryReadNumber(command.primary, out int src))
                    {
                        if(command.secondary.param == Param.ACC)
                        {
                            this.ACC = src;
                        }
                        if(command.secondary.param == Param.Direction)
                        {
                            BufferDirection(command.secondary.direction, src);
                        }
                    }
                    else
                    {
                        offset = 0;
                    }
                    break;
                case Instruction.SWP:
                    var swap = this.ACC;
                    this.ACC = this.BAK;
                    this.BAK = swap;
                    break;
                case Instruction.SAV:
                    this.BAK = this.ACC;
                    break;
                case Instruction.ADD:
                    if (TryReadNumber(command.primary, out int add))
                    {
                        this.ACC += add;
                    }
                    else
                    {
                        offset = 0;
                    }
                    break;
                case Instruction.SUB:
                    if (TryReadNumber(command.primary, out int sub))
                    {
                        this.ACC -= sub;
                    }
                    else
                    {
                        offset = 0;
                    }
                    break;
                case Instruction.NEG:
                    this.ACC = -this.ACC;
                    break;
                case Instruction.JMP:
                    offset = JumpOffset(() => true, command.primary.value);
                    break;
                case Instruction.JEZ:
                    offset = JumpOffset(() => this.ACC == 0, command.primary.value);
                    break;
                case Instruction.JNZ:
                    offset = JumpOffset(() => this.ACC != 0, command.primary.value);
                    break;
                case Instruction.JGZ:
                    offset = JumpOffset(() => this.ACC > 0, command.primary.value);
                    break;
                case Instruction.JLZ:
                    offset = JumpOffset(() => this.ACC < 0, command.primary.value);
                    break;
                case Instruction.JRO:
                    // output is zero if reading fails allowing looping
                    TryReadNumber(command.primary, out int jro);
                    offset += jro - 1;
                    break;
                default:
                    break;
            }

            // Jump to the instruction calculated
            // which may be 0
            this.PC += offset;
        }
    }
}
