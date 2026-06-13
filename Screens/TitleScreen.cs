using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Title screen </summary>
public sealed class TitleScreen : IScreen
{
    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (input.WasPressed(KeyCode.Escape))
        {
            return null;
        }

        return this;
    }

    public void Render(SdlPlatform platform)
    {
        BlockFont.DrawCenteredOutlined(platform, "LITTLE HADES", platform.Width, 200, 8, Color.Stairs, Color.Black);
        BlockFont.DrawCenteredOutlined(platform, "ESCAPE THE UNDERWORLD", platform.Width, 300, 3, Color.White, Color.Black);
        BlockFont.DrawCenteredOutlined(platform, "PRESS ESC TO QUIT", platform.Width, 420, 3, Color.Player, Color.Black);
    }
}
