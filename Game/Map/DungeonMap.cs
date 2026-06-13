using TheAdventure.Core;

namespace TheAdventure.Game.Map;

/// <summary> The tile grid plus per-cell visibility/explored state </summary>
public sealed class DungeonMap
{
    public DungeonMap(int width, int height)
    {
        Tiles = new Grid<TileType>(width, height, TileType.Wall);
        Visible = new Grid<bool>(width, height, false);
        Explored = new Grid<bool>(width, height, false);
    }

    public Grid<TileType> Tiles { get; }

    // Cells currently inside the field of view
    public Grid<bool> Visible { get; }

    // Cells the player has seen at least once
    public Grid<bool> Explored { get; }

    public int Width => Tiles.Width;
    public int Height => Tiles.Height;

    public bool InBounds(Position p) => Tiles.InBounds(p);

    public bool IsWalkable(Position p) => InBounds(p) && Tiles[p] != TileType.Wall;

    public bool BlocksSight(Position p) => !InBounds(p) || Tiles[p] == TileType.Wall;

    public Position? FindStairs()
    {
        foreach (var (pos, tile) in Tiles)
        {
            if (tile == TileType.StairsDown)
            {
                return pos;
            }
        }

        return null;
    }
}
