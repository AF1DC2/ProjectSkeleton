using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

public sealed class StrengthShard : Item
{
    private readonly int _bonus;

    public StrengthShard(int bonus = 1)
        : base("STRENGTH SHARD", '/', new Color(210, 130, 80))
    {
        _bonus = bonus;
    }

    public override void Apply(GameWorld world)
    {
        world.Player.IncreaseAttack(_bonus);
        world.Log.Add($"POWER SURGES THROUGH YOU. +{_bonus} ATK.");
    }
}
