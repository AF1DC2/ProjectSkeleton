using TheAdventure.Core;
using TheAdventure.Game.Entities;

namespace TheAdventure.Game;

/// <summary> Picks a monster type appropriate for the current depth (deeper = harder) </summary>
public static class MonsterFactory
{
    public static Monster CreateForDepth(int depth, Position position, Random rng)
    {
        var roll = rng.Next(100);

        return depth switch
        {
            <= 1 => roll < 80 ? new Shade(position) : new Skeleton(position),
            2 => roll < 55 ? new Shade(position)
                : roll < 90 ? new Skeleton(position)
                : new Wraith(position),
            _ => roll < 35 ? new Shade(position)
                : roll < 70 ? new Skeleton(position)
                : new Wraith(position),
        };
    }
}
