using TheAdventure.Core;
using TheAdventure.Game.Items;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Main Character </summary>
public sealed class Player : Actor
{
    public Player(Position position)
        : base("YOU", '@', position, maxHealth: 30, attackPower: 6, defense: 1, Color.Player)
    {
    }

    public int Gold { get; private set; }

    public Inventory Inventory { get; } = new();

    public int SightRadius => 7;

    public void AddGold(int amount) => Gold += Math.Max(0, amount);

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        Heal(amount);
    }

    public void IncreaseAttack(int amount) => AttackPower += amount;
}
