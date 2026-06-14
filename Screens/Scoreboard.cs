using TheAdventure.Persistence;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Renders a ranked list of score entries </summary>
public static class Scoreboard
{
    private static readonly Color RowColor = new(180, 180, 190);

    public static void Draw(SdlPlatform platform, IReadOnlyList<ScoreEntry> scores, int startY, ScoreEntry? highlight = null)
    {
        if (scores.Count == 0)
        {
            BlockFont.DrawCentered(platform, "NO SCORES YET", platform.Width, startY, 2, RowColor);
            return;
        }

        var y = startY;
        var rank = 1;
        foreach (var entry in scores)
        {
            var outcome = entry.Won ? "ESCAPED" : "DIED";
            var line = $"{rank}. {entry.Score}  FLOOR {entry.Depth}  {outcome}";
            var color = entry == highlight ? Color.Stairs : RowColor;
            BlockFont.DrawCentered(platform, line, platform.Width, y, 2, color);
            y += BlockFont.LineHeight(2) + 8;
            rank++;
        }
    }
}
