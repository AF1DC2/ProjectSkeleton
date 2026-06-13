using TheAdventure.Game;
using TheAdventure.Core;
using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Drives a live run, maps key presses to player turns and renders the world </summary>
public sealed class PlayScreen : IScreen
{
    public const int MapCols = 56;
    public const int MapRows = 36;

    private readonly GameWorld _world;

    public PlayScreen() => _world = new GameWorld(MapCols, MapRows);

    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (input.WasPressed(KeyCode.Escape))
        {
            return new TitleScreen();
        }

        if (_world.Status != GameStatus.Playing)
        {
            // Run finished
            if (input.AnyPressed)
            {
                return new TitleScreen();
            }

            return this;
        }

        HandleMovement(input);

        if (input.FirstPressed(KeyCode.Period, KeyCode.Return, KeyCode.KpEnter) is not null)
        {
            _world.Descend();
        }

        return this;
    }

    private void HandleMovement(InputState input)
    {
        var direction = input.FirstPressed(KeyCode.Up, KeyCode.W) is not null ? Direction.North
            : input.FirstPressed(KeyCode.Down, KeyCode.S) is not null ? Direction.South
            : input.FirstPressed(KeyCode.Left, KeyCode.A) is not null ? Direction.West
            : input.FirstPressed(KeyCode.Right, KeyCode.D) is not null ? Direction.East
            : Direction.None;

        if (direction != Direction.None)
        {
            _world.MovePlayer(direction);
        }
    }

    public void Render(SdlPlatform platform)
    {
        WorldView.Render(platform, _world);

        if (_world.Status == GameStatus.Won)
        {
            BlockFont.DrawCenteredOutlined(platform, "YOU ESCAPED!", platform.Width, 320, 6, Color.Stairs, Color.Black);
        }
        else if (_world.Status == GameStatus.Lost)
        {
            BlockFont.DrawCenteredOutlined(platform, "YOU DIED", platform.Width, 320, 6, Color.HealthBar, Color.Black);
        }
    }
}
