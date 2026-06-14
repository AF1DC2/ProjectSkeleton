using TheAdventure.Input;
using TheAdventure.Persistence;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Shows the saved high scores, loaded asynchronously from disk </summary>
public sealed class HighScoresScreen : IScreen
{
    private readonly Task<IReadOnlyList<ScoreEntry>> _loadTask = new HighScoreStore().LoadAsync();

    private IReadOnlyList<ScoreEntry>? _scores;
    private string? _error;

    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (_scores is null && _error is null && _loadTask.IsCompleted)
        {
            if (_loadTask.IsCompletedSuccessfully)
            {
                _scores = _loadTask.Result;
            }
            else
            {
                _error = (_loadTask.Exception?.InnerException as SaveGameException)?.Message
                         ?? "COULD NOT LOAD SCORES.";
            }
        }

        if (input.FirstPressed(KeyCode.Return, KeyCode.KpEnter, KeyCode.Escape, KeyCode.Space) is not null)
        {
            return new TitleScreen();
        }

        return this;
    }

    public void Render(SdlPlatform platform)
    {
        BlockFont.DrawCenteredOutlined(platform, "HIGH SCORES", platform.Width, 90, 5, Color.Gold, Color.Black);

        if (_error is not null)
        {
            BlockFont.DrawCentered(platform, _error, platform.Width, 240, 2, Color.HealthBar);
        }
        else if (_scores is null)
        {
            BlockFont.DrawCentered(platform, "LOADING.", platform.Width, 240, 2, Color.White);
        }
        else
        {
            Scoreboard.Draw(platform, _scores, 220);
        }

        BlockFont.DrawCentered(platform, "PRESS ENTER", platform.Width, platform.Height - 70, 3, Color.Player);
    }
}
