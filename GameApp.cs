using System.Diagnostics;
using TheAdventure.Input;
using TheAdventure.Rendering;
using TheAdventure.Screens;

namespace TheAdventure;

/// <summary>
/// The top-level game loop: input -> update -> render.
/// The loop ends when a screen returns null or the window is closed.
/// </summary>
public sealed class GameApp : IDisposable
{
    public const int WindowWidth = 880;
    public const int WindowHeight = 720;

    private readonly SdlPlatform _platform;
    private readonly InputState _input = new();

    public GameApp()
    {
        _platform = new SdlPlatform("Little Hades", WindowWidth, WindowHeight);
    }

    public void Run(IScreen initialScreen)
    {
        IScreen? screen = initialScreen;
        var timer = Stopwatch.StartNew();

        while (screen is not null)
        {
            var deltaSeconds = timer.Elapsed.TotalSeconds;
            timer.Restart();

            _platform.PumpEvents(_input);
            if (_input.QuitRequested)
            {
                break;
            }

            screen = screen.Update(_input, deltaSeconds);

            if (screen is not null)
            {
                _platform.Clear(Color.Background);
                screen.Render(_platform);
                _platform.Present();
            }
        }
    }

    public void Dispose() => _platform.Dispose();
}
