// AI-generated
using System.Collections;

namespace TheAdventure.Core;

public sealed class Grid<T> : IEnumerable<(Position Pos, T Value)>
{
    private readonly T[] _cells;

    public Grid(int width, int height, T initial)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Grid dimensions must be positive.");
        }

        Width = width;
        Height = height;
        _cells = new T[width * height];
        Array.Fill(_cells, initial);
    }

    public int Width { get; }
    public int Height { get; }

    public T this[int x, int y]
    {
        get => _cells[Index(x, y)];
        set => _cells[Index(x, y)] = value;
    }

    public T this[Position p]
    {
        get => this[p.X, p.Y];
        set => this[p.X, p.Y] = value;
    }

    public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

    public bool InBounds(Position p) => InBounds(p.X, p.Y);

    public void Fill(T value) => Array.Fill(_cells, value);

    private int Index(int x, int y)
    {
        if (!InBounds(x, y))
        {
            throw new ArgumentOutOfRangeException($"({x}, {y}) is outside the {Width}x{Height} grid.");
        }

        return y * Width + x;
    }

    public IEnumerator<(Position Pos, T Value)> GetEnumerator()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                yield return (new Position(x, y), _cells[y * Width + x]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
// end AI-generated
