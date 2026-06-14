using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Main menu: Play, High Scores or Quit with the arrow keys </summary>
public sealed class TitleScreen : IScreen
{
    private static readonly string[] Options = ["PLAY", "HIGH SCORES", "QUIT"];

    private int _selected;

    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (input.FirstPressed(KeyCode.Up, KeyCode.W) is not null)
        {
            _selected = (_selected - 1 + Options.Length) % Options.Length;
        }
        else if (input.FirstPressed(KeyCode.Down, KeyCode.S) is not null)
        {
            _selected = (_selected + 1) % Options.Length;
        }

        if (input.WasPressed(KeyCode.Escape))
        {
            return null;
        }

        if (input.FirstPressed(KeyCode.Return, KeyCode.KpEnter, KeyCode.Space) is not null)
        {
            return _selected switch
            {
                0 => new PlayScreen(),
                1 => new HighScoresScreen(),
                _ => null,
            };
        }

        return this;
    }

    public void Render(SdlPlatform platform)
    {
        BlockFont.DrawCenteredOutlined(platform, "LITTLE HADES", platform.Width, 120, 8, Color.Stairs, Color.Black);
        BlockFont.DrawCenteredOutlined(platform, "ESCAPE THE UNDERWORLD", platform.Width, 220, 3, Color.White, Color.Black);

        var y = 340;
        for (var i = 0; i < Options.Length; i++)
        {
            var selected = i == _selected;
            var label = selected ? $"> {Options[i]} <" : Options[i];
            var color = selected ? Color.Player : new Color(150, 150, 160);
            BlockFont.DrawCenteredOutlined(platform, label, platform.Width, y, 4, color, Color.Black);
            y += 64;
        }

        BlockFont.DrawCentered(platform, "ARROWS SELECT   ENTER CONFIRM", platform.Width, platform.Height - 70, 2, new Color(120, 120, 130));
    }
}
