// AI-generated
using TheAdventure.Core;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Base enemy, stands still until the hero is within sight then chases and bumps to attack </summary>
public abstract class Monster : Actor
{
    protected Monster(
        string name, char glyph, Position position, int maxHealth, int attackPower, int defense, Color color)
        : base(name, glyph, position, maxHealth, attackPower, defense, color)
    {
    }

    public virtual int SightRadius => 6;

    public abstract int Bounty { get; }

    // One AI step. Subclasses may override to move/attack differently
    public virtual void TakeTurn(GameWorld world)
    {
        if (!IsAlive)
        {
            return;
        }

        var target = world.Player.Position;

        if (Position.IsAdjacentTo(target))
        {
            world.ResolveAttack(this, world.Player);
            return;
        }

        if (Position.ManhattanDistanceTo(target) <= SightRadius)
        {
            StepTowards(world, target);
        }
    }

    // Greedily moves to the orthogonal neighbour that gets closest to the target
    protected void StepTowards(GameWorld world, Position target)
    {
        var current = Position.ManhattanDistanceTo(target);

        var step = Position.OrthogonalNeighbours()
            .Where(world.CanMonsterEnter)
            .Where(p => p.ManhattanDistanceTo(target) < current)
            .OrderBy(p => p.ManhattanDistanceTo(target))
            .Select(p => (Position?)p)
            .FirstOrDefault();

        if (step is { } next)
        {
            Position = next;
        }
    }
}
// end AI-generated
