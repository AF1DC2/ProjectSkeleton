namespace TheAdventure.Persistence;

/// <summary> Thrown when the high-score file cannot be read or written (missing, corrupt) </summary>
public sealed class SaveGameException : Exception
{
    public SaveGameException(string message)
        : base(message)
    {
    }

    public SaveGameException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
