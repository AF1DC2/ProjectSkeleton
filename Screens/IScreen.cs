using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Future game screen </summary>
public interface IScreen
{
    IScreen? Update(InputState input, double deltaSeconds);

    void Render(SdlPlatform platform);
}
