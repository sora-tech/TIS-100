using System.Collections.Generic;
using System.Linq;
using TIS100.Types;

/* The Validator
 * 
 * This performs 2 validation tasks at the same time.  The first is to 
 * check the options for each instruction are acceptable:
 * e.g. JMP TEXT is valid but JMP NUMBER is not.
 * The second is to check valid looking instructions with directions
 * are valid given the directions provided.
 * e.g. MOV 1 UP is only valid if the UP link is connected.
 * 
 * Validation being a separate step increases the complexity of a user
 * of this library but greatly simplifies the testing.
 */

namespace TIS100
{
    public static class Validator
    {
        private static bool ValidNumberParam(Param param)
        {
            return new List<Param> { Param.Direction, Param.Number, Param.ACC }.Contains(param);
        }

        public static List<Error> Validate(HashSet<Direction> links, List<Command> commands)
        {
            var errors = new List<Error>();

            foreach (var command in commands)
            {
                switch (command.instruction)
                {
                    case Instruction.NOP:
                    case Instruction.SWP:
                    case Instruction.SAV:
                    case Instruction.NEG:
                        // All these instructions have no options
                        if (command.primary.param != Param.Empty)
                        {
                            errors.Add(new Error(command, "invalid parameter", 0));
                        }
                        if (command.secondary.param != Param.Empty)
                        {
                            errors.Add(new Error(command, "invalid parameter", 1));
                        }
                        break;
                    case Instruction.MOV:
                        // The MOV instruction is the most complex as it takes 2 parameters
                        // which may be several combinations of Direction, Number or ACC
                        if (ValidNumberParam(command.primary.param) == false)
                        {
                            errors.Add(new Error(command, "invalid parameter", 0));
                        }

                        if (command.primary.param == Param.Direction)
                        {
                            if (links.Contains(command.primary.direction) == false)
                            {
                                errors.Add(new Error(command, "invalid direction", 0));
                            }
                        }

                        if (command.secondary.param == Param.Direction)
                        {
                            if (links.Contains(command.secondary.direction) == false)
                            {
                                errors.Add(new Error(command, "invalid direction", 1));
                            }
                        }
                        else if (command.secondary.param != Param.Direction && command.secondary.param != Param.ACC)
                        {
                            if (command.secondary.param != Param.Text || command.secondary.value != "NIL")
                            {
                                errors.Add(new Error(command, "invalid parameter", 1));
                            }
                        }

                        break;
                    case Instruction.ADD:
                    case Instruction.SUB:
                        // Both ADD and SUB must have a single option
                        // which maybe a NUMBER or a DIRECTION
                        if (command.secondary.param != Param.Empty)
                        {
                            errors.Add(new Error(command, "invalid parameter", 1));
                        }

                        if (ValidNumberParam(command.primary.param) == false)
                        {
                            errors.Add(new Error(command, "invalid parameter", 0));
                        }
                        else if (command.primary.param == Param.Direction)
                        {
                            if (links.Contains(command.primary.direction) == false)
                            {
                                errors.Add(new Error(command, "invalid direction", 0));
                            }
                        }
                        break;
                    case Instruction.JMP:
                    case Instruction.JEZ:
                    case Instruction.JNZ:
                    case Instruction.JGZ:
                    case Instruction.JLZ:
                        // The JUMP family except JRO must only have one option
                        // and the LABEL provided must exist in the given commands
                        if (command.primary.param != Param.Text)
                        {
                            errors.Add(new Error(command, "invalid parameter", 0));
                        }
                        else
                        {
                            var lblIndex = commands.FindIndex(x =>
                            x.instruction == Instruction.LBL
                            && x.primary.value == command.primary.value);

                            if (lblIndex == -1)
                            {
                                errors.Add(new Error(command, "invalid label", 0));
                            }
                        }
                        if (command.secondary.param != Param.Empty)
                        {
                            errors.Add(new Error(command, "invalid parameter", 1));
                        }
                        break;
                    case Instruction.JRO:
                        // JRO must have a single option which maybe a NUMBER
                        // or a valid DIRECTION
                        if (ValidNumberParam(command.primary.param) == false)
                        {
                            errors.Add(new Error(command, "invalid parameter", 0));
                        }
                        else if (command.primary.param == Param.Number)
                        {
                            var relative = command.primary.number;
                            var offest = command.line + relative;
                            if (offest < 0 || offest > commands.Max(x => x.line))
                            {
                                errors.Add(new Error(command, "invalid parameter", 0));
                            }
                        }

                        if (command.secondary.param != Param.Empty)
                        {
                            errors.Add(new Error(command, "invalid parameter", 1));
                        }
                        break;
                    case Instruction.LBL:
                        // Labels must be unique
                        var duplicates = commands.FindAll(x =>
                            x.instruction == Instruction.LBL
                            && x.primary.value == command.primary.value
                            && x.line != command.line);
                        foreach (var lblcmd in duplicates)
                        {
                            errors.Add(new Error(lblcmd, "duplicate label", 0));
                        }

                        break;

                    default:
                        break;
                }
            }

            return errors;
        }
    }
}
