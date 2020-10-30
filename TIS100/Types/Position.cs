namespace TIS100.Types
{
    /* Grid position named Tuple */
    public readonly struct Position
    {
        public readonly int width;
        public readonly int height;

        public Position(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
