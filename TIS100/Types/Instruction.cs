namespace TIS100.Types
{
    /* The Instructions supported by TIS 100 ASM */
    public enum Instruction
    {
        LBL,    //Label
        NOP,    //No-Op
        MOV,    //Move
        SWP,    //Swap
        SAV,    //Save
        ADD,    //Add
        SUB,    //Subtract
        NEG,    //Negate
        JMP,    //Jump
        JEZ,    //Jump Equals Zero
        JNZ,    //Jump Not Zero
        JGZ,    //Jump Greater than Zero
        JLZ,    //Jump Less than Zero
        JRO     //Jump Relative Offset
    }
}
