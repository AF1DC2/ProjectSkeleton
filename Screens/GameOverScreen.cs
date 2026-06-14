using TheAdventure.Game;
using TheAdventure.Input;
using TheAdventure.Persistence;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Records the result to disk (async) and lists the high scores </summary>
public sealed class GameOverScreen : IScreen
{
    private readonly bool _won;
    private readonly int _score;
    private readonly int _depth;
    private readonly ScoreEntry _entry;
    private readonly Task<IReadOnlyList<ScoreEntry>> _recordTask;

    private IReadOnlyList<ScoreEntry>? _scores;
    private string? _error;

    public GameOverScreen(GameWorld world)
    {
        _won = world.Status == GameStatus.Won;
        _score = world.Score;
        _depth = world.Depth;

        _entry = new ScoreEntry(_score, _depth, _won, DateTime.UtcNow);
        _recordTask = new HighScoreStore().RecordAsync(_entry);
    }

    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (_scores is null && _error is null && _recordTask.IsCompleted)
        {
            if (_recordTask.IsCompletedSuccessfully)
            {
                _scores = _recordTask.Result;
            }
            else
            {
                _error = (_recordTask.Exception?.InnerException as SaveGameException)?.Message
                         ?? "COULD NOT SAVE YOUR SCORE.";
            }
        }

        if (input.WasPressed(KeyCode.R))
        {
            return new PlayScreen();
        }

        if (input.FirstPressed(KeyCode.Return, KeyCode.KpEnter, KeyCode.Escape, KeyCode.Space) is not null)
        {
            return new TitleScreen();
        }

        return this;
    }

    public void Render(SdlPlatform platform)
    {
        var title = _won ? "YOU ESCAPED!" : "YOU DIED";
        var titleColor = _won ? Color.Stairs : Color.HealthBar;
        BlockFont.DrawCenteredOutlined(platform, title, platform.Width, 70, 6, titleColor, Color.Black);
        BlockFont.DrawCenteredOutlined(
            platform, $"SCORE {_score}   FLOOR {_depth}", platform.Width, 150, 3, Color.White, Color.Black);

        BlockFont.DrawCentered(platform, "HIGH SCORES", platform.Width, 220, 3, Color.Gold);

        if (_error is not null)
        {
            BlockFont.DrawCentered(platform, _error, platform.Width, 280, 2, Color.HealthBar);
        }
        else if (_scores is null)
        {
            BlockFont.DrawCentered(platform, "SAVING.", platform.Width, 280, 2, Color.White);
        }
        else
        {
            Scoreboard.Draw(platform, _scores, 270, _entry);
        }

        BlockFont.DrawCentered(platform, "ENTER  MENU      R  PLAY AGAIN", platform.Width, platform.Height - 56, 3, Color.Player);
    }
}
