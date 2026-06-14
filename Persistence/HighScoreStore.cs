using System.Text.Json;

namespace TheAdventure.Persistence;

/// <summary> Reads and writes the top-scores list to a JSON file in the user's app-data folder </summary>
public sealed class HighScoreStore
{
    public const int MaxEntries = 10;

    private readonly string _filePath;

    public HighScoreStore(string? filePath = null) => _filePath = filePath ?? DefaultPath();

    public string FilePath => _filePath;

    private static string DefaultPath()
    {
        var directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LittleHades");
        return Path.Combine(directory, "highscores.json");
    }

    // Loads the saved scores
    public async Task<IReadOnlyList<ScoreEntry>> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return Array.Empty<ScoreEntry>();
        }

        try
        {
            await using var stream = File.OpenRead(_filePath);
            var entries = await JsonSerializer.DeserializeAsync(stream, ScoreSerializerContext.Default.ListScoreEntry);
            return entries ?? new List<ScoreEntry>();
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            throw new SaveGameException($"Could not read high scores from '{_filePath}'.", ex);
        }
    }

    // Adds an entry
    public async Task<IReadOnlyList<ScoreEntry>> RecordAsync(ScoreEntry entry)
    {
        IReadOnlyList<ScoreEntry> existing;
        try
        {
            existing = await LoadAsync();
        }
        catch (SaveGameException)
        {
            existing = Array.Empty<ScoreEntry>();
        }

        var updated = existing
            .Append(entry)
            .OrderByDescending(e => e.Score)
            .ThenByDescending(e => e.Depth)
            .Take(MaxEntries)
            .ToList();

        await SaveAsync(updated);
        return updated;
    }

    private async Task SaveAsync(IReadOnlyList<ScoreEntry> entries)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, entries.ToList(), ScoreSerializerContext.Default.ListScoreEntry);
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            throw new SaveGameException($"Could not write high scores to '{_filePath}'.", ex);
        }
    }
}
