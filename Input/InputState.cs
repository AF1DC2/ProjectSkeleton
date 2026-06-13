namespace TheAdventure.Input;

/// <summary> Collects the key presses that happened during a single frame.
/// This is filled from SDL key-down events (keyboard auto-repeat included) </summary>
public sealed class InputState
{
    private readonly HashSet<KeyCode> _pressedThisFrame = new();

    public bool QuitRequested { get; set; }

    public void BeginFrame() => _pressedThisFrame.Clear();

    public void RegisterKeyDown(KeyCode key) => _pressedThisFrame.Add(key);

    public bool WasPressed(KeyCode key) => _pressedThisFrame.Contains(key);

    public KeyCode? FirstPressed(params KeyCode[] keys) =>
        keys.Cast<KeyCode?>().FirstOrDefault(k => _pressedThisFrame.Contains(k!.Value));

    public bool AnyPressed => _pressedThisFrame.Count > 0;
}
