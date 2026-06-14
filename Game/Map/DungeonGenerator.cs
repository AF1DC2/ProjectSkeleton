// AI-generated
using TheAdventure.Core;

namespace TheAdventure.Game.Map;

/// <summary> Non-overlapping rooms generator linked with L-shaped tunnels </summary>
public sealed class DungeonGenerator
{
    private const int MaxRooms = 14;
    private const int MinRoomSize = 5;
    private const int MaxRoomSize = 10;

    private readonly Random _rng;

    public DungeonGenerator(Random rng) => _rng = rng;

    public GeneratedLevel Generate(int width, int height)
    {
        var map = new DungeonMap(width, height);
        var rooms = new List<Room>();

        for (var attempt = 0; attempt < MaxRooms * 3 && rooms.Count < MaxRooms; attempt++)
        {
            var w = _rng.Next(MinRoomSize, MaxRoomSize + 1);
            var h = _rng.Next(MinRoomSize, MaxRoomSize + 1);
            var x = _rng.Next(1, width - w - 1);
            var y = _rng.Next(1, height - h - 1);
            var room = new Room(x, y, w, h);

            if (rooms.Any(existing => existing.Intersects(room)))
            {
                continue;
            }

            CarveRoom(map, room);

            if (rooms.Count > 0)
            {
                CarveCorridor(map, rooms[^1].Center, room.Center);
            }

            rooms.Add(room);
        }

        if (rooms.Count < 2)
        {
            throw new InvalidOperationException("Dungeon generation failed to place enough rooms.");
        }

        // Stairs sit in the last room
        map.Tiles[rooms[^1].Center] = TileType.StairsDown;

        return new GeneratedLevel(map, rooms[0].Center, rooms);
    }

    private static void CarveRoom(DungeonMap map, Room room)
    {
        foreach (var cell in room.InteriorCells())
        {
            map.Tiles[cell] = TileType.Floor;
        }
    }

    private void CarveCorridor(DungeonMap map, Position from, Position to)
    {
        if (_rng.Next(2) == 0)
        {
            CarveHorizontal(map, from.X, to.X, from.Y);
            CarveVertical(map, from.Y, to.Y, to.X);
        }
        else
        {
            CarveVertical(map, from.Y, to.Y, from.X);
            CarveHorizontal(map, from.X, to.X, to.Y);
        }
    }

    private static void CarveHorizontal(DungeonMap map, int x1, int x2, int y)
    {
        foreach (var x in Range(x1, x2))
        {
            var p = new Position(x, y);
            if (map.InBounds(p) && map.Tiles[p] == TileType.Wall)
            {
                map.Tiles[p] = TileType.Floor;
            }
        }
    }

    private static void CarveVertical(DungeonMap map, int y1, int y2, int x)
    {
        foreach (var y in Range(y1, y2))
        {
            var p = new Position(x, y);
            if (map.InBounds(p) && map.Tiles[p] == TileType.Wall)
            {
                map.Tiles[p] = TileType.Floor;
            }
        }
    }

    private static IEnumerable<int> Range(int a, int b)
    {
        var (lo, hi) = a <= b ? (a, b) : (b, a);
        for (var i = lo; i <= hi; i++)
        {
            yield return i;
        }
    }
}
// end AI-generated
