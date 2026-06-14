using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Interface between the game loop and game content </summary>
public interface IScreen
{
    IScreen? Update(InputState input, double deltaSeconds);

    void Render(SdlPlatform platform);
}
