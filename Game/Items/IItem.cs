using TheAdventure.Rendering;

namespace TheAdventure.Game.Items;

public interface IItem
{
    string Name { get; }
    char Glyph { get; }
    Color Color { get; }

    bool ConsumeOnPickup { get; }

    void Apply(GameWorld world);
}
