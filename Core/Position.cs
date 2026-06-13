namespace TheAdventure.Core;

/// <summary> Used code from https://codereview.stackexchange.com/questions/120933/calculating-distance-with-euclidean-manhattan-and-chebyshev-in-c </summary>
public readonly record struct Position(int X, int Y)
{
    public Position Step(Direction direction) => direction switch
    {
        Direction.North => this with { Y = Y - 1 },
        Direction.South => this with { Y = Y + 1 },
        Direction.West => this with { X = X - 1 },
        Direction.East => this with { X = X + 1 },
        _ => this,
    };

    public int ChebyshevDistanceTo(Position other) =>
        Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));

    public int ManhattanDistanceTo(Position other) =>
        Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

    public bool IsAdjacentTo(Position other) =>
        this != other && ChebyshevDistanceTo(other) <= 1;

    public IEnumerable<Position> OrthogonalNeighbours()
    {
        yield return Step(Direction.North);
        yield return Step(Direction.South);
        yield return Step(Direction.West);
        yield return Step(Direction.East);
    }
}

public enum Direction
{
    None,
    North,
    South,
    West,
    East,
}
