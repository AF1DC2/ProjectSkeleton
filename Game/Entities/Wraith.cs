using TheAdventure.Core;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Takes two AI steps per turn, so it closes in and strikes quickly </summary>
public sealed class Wraith : Monster
{
    public Wraith(Position position)
        : base("WRAITH", 'W', position, maxHealth: 12, attackPower: 6, defense: 1, new Color(170, 90, 200))
    {
    }

    public override int Bounty => 35;

    public override void TakeTurn(GameWorld world)
    {
        base.TakeTurn(world);
        if (IsAlive && world.Player.IsAlive)
        {
            base.TakeTurn(world);
        }
    }
}
