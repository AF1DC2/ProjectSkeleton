namespace TheAdventure.Persistence;

/// <summary> One persisted result of a finished run </summary>
public sealed record ScoreEntry(int Score, int Depth, bool Won, DateTime DateUtc);
