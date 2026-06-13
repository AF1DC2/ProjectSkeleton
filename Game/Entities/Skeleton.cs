using TheAdventure.Core;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Tougher melee enemy with a wider sight range </summary>
public sealed class Skeleton : Monster
{
    public Skeleton(Position position)
        : base("SKELETON", 'K', position, maxHealth: 14, attackPower: 5, defense: 1, new Color(220, 220, 200))
    {
    }

    public override int SightRadius => 8;

    public override int Bounty => 20;
}
