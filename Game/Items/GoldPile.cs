using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

public sealed class GoldPile : Item
{
    private readonly int _amount;

    public GoldPile(int amount)
        : base("GOLD", '*', Color.Gold)
    {
        _amount = amount;
    }

    public override void Apply(GameWorld world)
    {
        world.CollectGold(_amount);
        world.Log.Add($"YOU GATHER {_amount} GOLD.");
    }
}
