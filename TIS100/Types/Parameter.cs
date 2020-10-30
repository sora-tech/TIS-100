namespace TIS100.Types
{
    /* Strongly typed store for parsed symbols */
    public enum Param
    {
        Empty,
        Label,
        Text,
        Number,
        ACC,
        Direction
    }

    public readonly struct Parameter
    {
        public readonly Param param;
        public readonly string value;
        public readonly int number;
        public readonly Direction direction;

        public Parameter(Param param, string value)
        {
            this.param = param;
            this.value = value;
            this.number = 0;
            this.direction = Direction.DOWN;
        }

        public Parameter(int value)
        {
            this.param = Param.Number;
            this.number = value;
            this.value = string.Empty;
            this.direction = Direction.UP;
        }

        public Parameter(Param param)
        {
            this.param = param;
            this.number = 0;
            this.value = string.Empty;
            this.direction = Direction.UP;
        }

        public Parameter(Direction dir)
        {
            this.param = Param.Direction;
            this.number = 0;
            this.value = string.Empty;
            this.direction = dir;
        }
    }
}
