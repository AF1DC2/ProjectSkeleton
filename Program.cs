using TheAdventure.Screens;

namespace TheAdventure;

public static class Program
{
    public static void Main()
    {
        using var app = new GameApp();
        app.Run(new TitleScreen());
    }
}
