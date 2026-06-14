using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

public sealed class VitalityHeart : Item
{
    private readonly int _bonus;

    public VitalityHeart(int bonus = 5)
        : base("VITALITY HEART", '+', new Color(220, 80, 90))
    {
        _bonus = bonus;
    }

    public override void Apply(GameWorld world)
    {
        world.Player.IncreaseMaxHealth(_bonus);
        world.Log.Add($"YOUR VIGOUR GROWS. +{_bonus} MAX HP.");
    }
}
