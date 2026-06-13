using TheAdventure.Game.Map;
using TheAdventure.Input;
using TheAdventure.Rendering;

namespace TheAdventure.Screens;

/// <summary> Generates a dungeon floor and draws it to verify generator </summary>
public sealed class PlayScreen : IScreen
{
    public const int MapCols = 56;
    public const int MapRows = 36;
    private const int TileSize = 15;

    private GeneratedLevel _level;

    public PlayScreen()
    {
        _level = new DungeonGenerator(new Random()).Generate(MapCols, MapRows);
    }

    public IScreen? Update(InputState input, double deltaSeconds)
    {
        if (input.WasPressed(KeyCode.Escape))
        {
            return new TitleScreen();
        }

        if (input.WasPressed(KeyCode.R))
        {
            _level = new DungeonGenerator(new Random()).Generate(MapCols, MapRows);
        }

        return this;
    }

    public void Render(SdlPlatform platform)
    {
        var originX = (platform.Width - MapCols * TileSize) / 2;
        var originY = (platform.Height - MapRows * TileSize) / 2;

        foreach (var (pos, tile) in _level.Map.Tiles)
        {
            var color = tile switch
            {
                TileType.Wall => Color.Wall,
                TileType.Floor => Color.Floor,
                TileType.StairsDown => Color.Stairs,
                _ => Color.Background,
            };

            platform.FillRect(originX + pos.X * TileSize, originY + pos.Y * TileSize, TileSize - 1, TileSize - 1, color);
        }

        BlockFont.DrawOutlined(platform, "R REGEN   ESC MENU", 12, 12, 2, Color.White, Color.Black);
    }
}
