namespace ShisenSho.Common
{
    public static class Emojis
    {
        public static readonly string[] AvailableEmojis = {
            "🤣",
            "😀",
            "😉",
            "😍",
            "🤑",
            "🤭",
            "🤔",
            "😑",
            "😴",
            "😵‍💫",
            "🤯",
            "😎",
            "🤓",
            "🥺",
            "😭",
            "😱",
            "😩",
            "😤",
            "😡",
            "😈"
        };
    }

    enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class TileValue
    {
        public const int Uninitialized = -2;
        public const int OutOfBounds = -1;
        public const int Empty = 0;
        public const int ForceEmpty = 1;
        public const int Wall = 2;
        public const int Border = 3;
        public const int TileOffset = 4;
    }

    public struct Coordinate
    {
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (left.x == right.x && left.y == right.y) { return true; } return false;
        }
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }
        public static Coordinate operator +(Coordinate left, Coordinate right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }
    }

    public static class RandomInstance
    {
        private static readonly Random _random = new Random();
        
        public static int RandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static T RandomObject<T>(IList<T> collection)
        {
            int index = RandomInt(0, collection.Count - 1);
            return collection[index];
        }
    }
}
