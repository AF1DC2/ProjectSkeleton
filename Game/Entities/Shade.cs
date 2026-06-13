using TheAdventure.Core;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Weak, common enemy </summary>
public sealed class Shade : Monster
{
    public Shade(Position position)
        : base("SHADE", 'S', position, maxHealth: 8, attackPower: 3, defense: 0, new Color(120, 140, 180))
    {
    }

    public override int Bounty => 10;
}
