namespace TIS100.Types
{
    /* Simple value store for lexed symbols */
    public readonly struct Symbol
    {
        public readonly Token token;
        public readonly string value;

        public Symbol(Token token, string value)
        {
            this.token = token;
            this.value = value;
        }
    }
}
