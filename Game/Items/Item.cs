using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

/// <summary> Shared base for items: holds the display data, leaves the effect to subclasses </summary>
public abstract class Item : IItem
{
    protected Item(string name, char glyph, Color color)
    {
        Name = name;
        Glyph = glyph;
        Color = color;
    }

    public string Name { get; }
    public char Glyph { get; }
    public Color Color { get; }

    public virtual bool ConsumeOnPickup => true;

    public abstract void Apply(GameWorld world);
}
