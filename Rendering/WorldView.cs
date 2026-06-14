// AI-generated
using TheAdventure.Core;
using TheAdventure.Game;
using TheAdventure.Game.Entities;
using TheAdventure.Game.Items;
using TheAdventure.Game.Map;

namespace TheAdventure.Rendering;

/// <summary> Renders the fog-of-war map, the entities, and the HUD </summary>
public static class WorldView
{
    private const int HudTop = 64;
    private const int LogHeight = 96;

    public static void Render(SdlPlatform platform, GameWorld world)
    {
        var map = world.Map;
        var availWidth = platform.Width;
        var availHeight = platform.Height - HudTop - LogHeight;
        var tile = Math.Min(availWidth / map.Width, availHeight / map.Height);
        var originX = (availWidth - map.Width * tile) / 2;
        var originY = HudTop + (availHeight - map.Height * tile) / 2;

        DrawMap(platform, world, originX, originY, tile);
        DrawItems(platform, world, originX, originY, tile);
        DrawActors(platform, world, originX, originY, tile);
        DrawHud(platform, world);
        DrawLog(platform, world);

        BlockFont.Draw(
            platform, "WASD MOVE   Q DRINK   ENTER DESCEND   ESC MENU",
            12, platform.Height - 18, 2, new Color(110, 110, 125));
    }

    private static void DrawItems(SdlPlatform platform, GameWorld world, int originX, int originY, int tile)
    {
        foreach (var (pos, item) in world.FloorItems)
        {
            if (world.Map.Visible[pos])
            {
                DrawGlyphAt(platform, item.Glyph, item.Color, pos, originX, originY, tile);
            }
        }
    }

    private static void DrawMap(SdlPlatform platform, GameWorld world, int originX, int originY, int tile)
    {
        var map = world.Map;
        foreach (var (pos, type) in map.Tiles)
        {
            if (!map.Explored[pos])
            {
                continue;
            }

            var baseColor = type switch
            {
                TileType.Wall => Color.Wall,
                TileType.Floor => Color.Floor,
                TileType.StairsDown => Color.Stairs,
                _ => Color.Background,
            };

            var color = map.Visible[pos] ? baseColor : baseColor.Dimmed(0.4);
            platform.FillRect(originX + pos.X * tile, originY + pos.Y * tile, tile - 1, tile - 1, color);
        }
    }

    private static void DrawActors(SdlPlatform platform, GameWorld world, int originX, int originY, int tile)
    {
        foreach (var monster in world.Monsters)
        {
            if (world.Map.Visible[monster.Position])
            {
                DrawGlyph(platform, monster, originX, originY, tile);
            }
        }

        DrawGlyphAt(platform, world.Player.Glyph, world.Player.Color, world.Player.Position, originX, originY, tile);
    }

    private static void DrawGlyph(SdlPlatform platform, Actor actor, int originX, int originY, int tile) =>
        DrawGlyphAt(platform, actor.Glyph, actor.Color, actor.Position, originX, originY, tile);

    private static void DrawGlyphAt(
        SdlPlatform platform, char glyphChar, Color color, Position pos, int originX, int originY, int tile)
    {
        var scale = Math.Max(1, tile / 6);
        var glyph = glyphChar.ToString();
        var glyphWidth = BlockFont.MeasureWidth(glyph, scale);
        var glyphHeight = BlockFont.LineHeight(scale);
        var x = originX + pos.X * tile + (tile - glyphWidth) / 2;
        var y = originY + pos.Y * tile + (tile - glyphHeight) / 2;
        BlockFont.DrawOutlined(platform, glyph, x, y, scale, color, Color.Black);
    }

    private static void DrawHud(SdlPlatform platform, GameWorld world)
    {
        var player = world.Player;
        BlockFont.Draw(platform, $"FLOOR {world.Depth}/{GameWorld.MaxDepth}", 12, 10, 3, Color.White);

        // Health bar.
        const int barX = 12;
        const int barY = 36;
        const int barW = 180;
        const int barH = 14;
        platform.FillRect(barX, barY, barW, barH, new Color(50, 20, 20));
        var fill = (int)(barW * (player.Health / (double)player.MaxHealth));
        platform.FillRect(barX, barY, fill, barH, Color.HealthBar);
        BlockFont.Draw(platform, $"HP {player.Health}/{player.MaxHealth}", barX + barW + 12, barY + 2, 2, Color.White);

        BlockFont.Draw(platform, $"SCORE {world.Score}", 520, 10, 3, Color.Stairs);
        BlockFont.Draw(platform, $"GOLD {player.Gold}", 520, 40, 2, Color.Gold);
        BlockFont.Draw(platform, $"POTIONS {player.Inventory.CountOf<HealthPotion>()} [Q]", 700, 40, 2, Color.Potion);
    }

    private static void DrawLog(SdlPlatform platform, GameWorld world)
    {
        var y = platform.Height - LogHeight + 6;
        foreach (var message in world.Log.Recent(3))
        {
            BlockFont.Draw(platform, message, 12, y, 2, new Color(170, 170, 180));
            y += BlockFont.LineHeight(2) + 6;
        }
    }
}
// end AI-generated
