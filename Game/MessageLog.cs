namespace TheAdventure.Game;

/// <summary> The HUD shows the most recent few lines </summary>
public sealed class MessageLog
{
    private readonly List<string> _messages = new();

    public void Add(string message) => _messages.Add(message);

    public IReadOnlyList<string> Recent(int count) =>
        _messages.TakeLast(count).ToList();
}
