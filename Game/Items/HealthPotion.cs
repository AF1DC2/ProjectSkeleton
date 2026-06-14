using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

public sealed class HealthPotion : Item
{
    private readonly int _healAmount;

    public HealthPotion(int healAmount = 12)
        : base("HEALTH POTION", '!', Color.Potion)
    {
        _healAmount = healAmount;
    }

    public override bool ConsumeOnPickup => false;

    public override void Apply(GameWorld world)
    {
        world.Player.Heal(_healAmount);
        world.Log.Add($"YOU QUAFF A POTION. +{_healAmount} HP.");
    }
}
