namespace TheAdventure.Rendering;

/// <summary> A simple opaque-by-default RGBA colour </summary>
public readonly record struct Color(byte R, byte G, byte B, byte A = 255)
{
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(235, 235, 235);
    public static readonly Color Background = new(16, 16, 22);
    public static readonly Color Wall = new(70, 70, 92);
    public static readonly Color WallDim = new(34, 34, 48);
    public static readonly Color Floor = new(40, 40, 52);
    public static readonly Color FloorDim = new(22, 22, 30);
    public static readonly Color Player = new(90, 200, 90);
    public static readonly Color Stairs = new(230, 200, 80);
    public static readonly Color Gold = new(235, 200, 70);
    public static readonly Color Potion = new(210, 80, 140);
    public static readonly Color HealthBar = new(200, 70, 70);

    public Color Dimmed(double factor) => new(
        (byte)(R * factor),
        (byte)(G * factor),
        (byte)(B * factor),
        A);
}
