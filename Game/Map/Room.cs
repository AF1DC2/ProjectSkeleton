using TheAdventure.Core;

namespace TheAdventure.Game.Map;

/// <summary> Rectangular room used by the generator (inclusive top-left, exclusive size) </summary>
public readonly record struct Room(int X, int Y, int Width, int Height)
{
    public int Right => X + Width;
    public int Bottom => Y + Height;

    public Position Center => new(X + Width / 2, Y + Height / 2);

    public bool Intersects(Room other) =>
        X - 1 < other.Right && Right + 1 > other.X &&
        Y - 1 < other.Bottom && Bottom + 1 > other.Y;

    public IEnumerable<Position> InteriorCells()
    {
        for (var y = Y; y < Bottom; y++)
        {
            for (var x = X; x < Right; x++)
            {
                yield return new Position(x, y);
            }
        }
    }
}
