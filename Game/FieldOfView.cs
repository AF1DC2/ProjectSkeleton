// AI-generated
using TheAdventure.Core;
using TheAdventure.Game.Map;

namespace TheAdventure.Game;

/// <summary> Field of view: a cell is visible if it is within range and unobstructed. </summary>
public static class FieldOfView
{
    public static void Compute(DungeonMap map, Position origin, int radius)
    {
        map.Visible.Fill(false);

        for (var y = origin.Y - radius; y <= origin.Y + radius; y++)
        {
            for (var x = origin.X - radius; x <= origin.X + radius; x++)
            {
                var cell = new Position(x, y);
                if (!map.InBounds(cell) || origin.ChebyshevDistanceTo(cell) > radius)
                {
                    continue;
                }

                if (HasLineOfSight(map, origin, cell))
                {
                    map.Visible[cell] = true;
                    map.Explored[cell] = true;
                }
            }
        }
    }

    /// <summary> Sight is blocked only by walls strictly between the endpoints </summary>
    private static bool HasLineOfSight(DungeonMap map, Position from, Position to)
    {
        int x0 = from.X, y0 = from.Y;
        int dx = Math.Abs(to.X - x0), dy = Math.Abs(to.Y - y0);
        int sx = x0 < to.X ? 1 : -1, sy = y0 < to.Y ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            if (x0 == to.X && y0 == to.Y)
            {
                return true;
            }

            // A wall between the endpoints blocks the line (the wall cell itself stays visible)
            if (!(x0 == from.X && y0 == from.Y) && map.BlocksSight(new Position(x0, y0)))
            {
                return false;
            }

            var e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}
// end AI-generated
