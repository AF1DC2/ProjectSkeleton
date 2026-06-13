using TheAdventure.Core;
using TheAdventure.Rendering;

namespace TheAdventure.Game.Entities;

/// <summary> Base class for anything that lives on the grid </summary>
public abstract class Actor
{
    protected Actor(string name, char glyph, Position position, int maxHealth, int attackPower, int defense, Color color)
    {
        Name = name;
        Glyph = glyph;
        Position = position;
        MaxHealth = maxHealth;
        Health = maxHealth;
        AttackPower = attackPower;
        Defense = defense;
        Color = color;
    }

    public string Name { get; }
    public char Glyph { get; }
    public Position Position { get; set; }
    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }
    public int AttackPower { get; protected set; }
    public int Defense { get; protected set; }
    public Color Color { get; }

    public bool IsAlive => Health > 0;

    public void Heal(int amount) => Health = Math.Min(MaxHealth, Health + Math.Max(0, amount));

    public void TakeDamage(int amount) => Health = Math.Max(0, Health - Math.Max(0, amount));
}
