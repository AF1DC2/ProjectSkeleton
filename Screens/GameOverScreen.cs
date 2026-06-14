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
    private readonly Task<IReadOnlyList<ScoreEntry>> _recordTask;

    private IReadOnlyList<ScoreEntry>? _scores;
    private string? _error;

    public GameOverScreen(GameWorld world)
    {
        _won = world.Status == GameStatus.Won;
        _score = world.Score;
        _depth = world.Depth;

        var entry = new ScoreEntry(_score, _depth, _won, DateTime.UtcNow);
        _recordTask = new HighScoreStore().RecordAsync(entry);
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
            DrawScoreTable(platform);
        }

        BlockFont.DrawCentered(platform, "PRESS ENTER", platform.Width, platform.Height - 60, 3, Color.Player);
    }

    private void DrawScoreTable(SdlPlatform platform)
    {
        var y = 270;
        var rank = 1;
        foreach (var entry in _scores!)
        {
            var outcome = entry.Won ? "ESCAPED" : "DIED";
            var line = $"{rank}. {entry.Score}  FLOOR {entry.Depth}  {outcome}";
            var color = entry.Score == _score && entry.Depth == _depth ? Color.Stairs : new Color(180, 180, 190);
            BlockFont.DrawCentered(platform, line, platform.Width, y, 2, color);
            y += BlockFont.LineHeight(2) + 8;
            rank++;
        }
    }
}
