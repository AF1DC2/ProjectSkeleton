using TheAdventure.Game.Items;

namespace TheAdventure.Game;

/// <summary> Rolls a random item to drop (or nothing). Health potions get more common with level </summary>
public static class ItemFactory
{
    public static Item? Roll(int depth, Random rng)
    {
        var r = rng.Next(100);

        return r switch
        {
            < 45 => new GoldPile(rng.Next(5, 16)),
            < 70 => new HealthPotion(),
            < 82 => new StrengthShard(),
            < 90 => new VitalityHeart(),
            _ => null,
        };
    }
}
