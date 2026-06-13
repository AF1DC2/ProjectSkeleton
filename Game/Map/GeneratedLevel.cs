using TheAdventure.Core;

namespace TheAdventure.Game.Map;

public sealed record GeneratedLevel(DungeonMap Map, Position PlayerStart, IReadOnlyList<Room> Rooms);
