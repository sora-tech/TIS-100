namespace TIS100.Types
{
    /* Fully parsed commands with the instruction and optional parameters */
    public readonly struct Command
    {
        public readonly int line;
        public readonly Instruction instruction;
        public readonly Parameter primary;
        public readonly Parameter secondary;

        public Command(Instruction instruction, int line) : this(instruction, line, null, null) { }
        public Command(Instruction instruction, int line, Parameter? primary) : this(instruction, line, primary, null) { }
        public Command(Instruction instruction, int line, Parameter? primary, Parameter? secondary)
        {
            this.line = line;
            this.instruction = instruction;
            this.primary = primary ?? new Parameter(Param.Empty);
            this.secondary = secondary ?? new Parameter(Param.Empty);
        }
    }
}
