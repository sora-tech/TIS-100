namespace TIS100.Types
{
    /* Errors produced by the Validator */
    public readonly struct Error
    {
        public readonly Command command;
        public readonly int line;
        public readonly int token;

        public readonly string message; 
        /* Message here is a translation/QA risk. An error code and lookup would
         * allow more flexability but greater complexity
        */

        public Error(Command command, string message, int token)
        {
            this.command = command;
            line = command.line;
            this.message = message;
            this.token = token;
        }
    }
}
