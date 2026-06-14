// AI-generated
namespace TheAdventure.Rendering;

/// <summary> Bitmap font drawn with filled rectangles </summary>
public static class BlockFont
{
    public const int GlyphWidth = 3;
    public const int GlyphHeight = 5;

    private static readonly Dictionary<char, string[]> Glyphs = new()
    {
        ['A'] = ["###", "#.#", "###", "#.#", "#.#"],
        ['B'] = ["##.", "#.#", "##.", "#.#", "##."],
        ['C'] = ["###", "#..", "#..", "#..", "###"],
        ['D'] = ["##.", "#.#", "#.#", "#.#", "##."],
        ['E'] = ["###", "#..", "###", "#..", "###"],
        ['F'] = ["###", "#..", "###", "#..", "#.."],
        ['G'] = ["###", "#..", "#.#", "#.#", "###"],
        ['H'] = ["#.#", "#.#", "###", "#.#", "#.#"],
        ['I'] = ["###", ".#.", ".#.", ".#.", "###"],
        ['J'] = ["..#", "..#", "..#", "#.#", "###"],
        ['K'] = ["#.#", "#.#", "##.", "#.#", "#.#"],
        ['L'] = ["#..", "#..", "#..", "#..", "###"],
        ['M'] = ["#.#", "###", "###", "#.#", "#.#"],
        ['N'] = ["#.#", "###", "###", "###", "#.#"],
        ['O'] = ["###", "#.#", "#.#", "#.#", "###"],
        ['P'] = ["###", "#.#", "###", "#..", "#.."],
        ['Q'] = ["###", "#.#", "#.#", "###", ".##"],
        ['R'] = ["###", "#.#", "##.", "#.#", "#.#"],
        ['S'] = ["###", "#..", "###", "..#", "###"],
        ['T'] = ["###", ".#.", ".#.", ".#.", ".#."],
        ['U'] = ["#.#", "#.#", "#.#", "#.#", "###"],
        ['V'] = ["#.#", "#.#", "#.#", "#.#", ".#."],
        ['W'] = ["#.#", "#.#", "###", "###", "#.#"],
        ['X'] = ["#.#", "#.#", ".#.", "#.#", "#.#"],
        ['Y'] = ["#.#", "#.#", ".#.", ".#.", ".#."],
        ['Z'] = ["###", "..#", ".#.", "#..", "###"],
        ['0'] = ["###", "#.#", "#.#", "#.#", "###"],
        ['1'] = [".#.", "##.", ".#.", ".#.", "###"],
        ['2'] = ["###", "..#", "###", "#..", "###"],
        ['3'] = ["###", "..#", "###", "..#", "###"],
        ['4'] = ["#.#", "#.#", "###", "..#", "..#"],
        ['5'] = ["###", "#..", "###", "..#", "###"],
        ['6'] = ["###", "#..", "###", "#.#", "###"],
        ['7'] = ["###", "..#", ".#.", ".#.", ".#."],
        ['8'] = ["###", "#.#", "###", "#.#", "###"],
        ['9'] = ["###", "#.#", "###", "..#", "###"],
        [':'] = ["...", ".#.", "...", ".#.", "..."],
        ['.'] = ["...", "...", "...", "...", ".#."],
        ['!'] = [".#.", ".#.", ".#.", "...", ".#."],
        ['-'] = ["...", "...", "###", "...", "..."],
        ['+'] = ["...", ".#.", "###", ".#.", "..."],
        ['/'] = ["..#", "..#", ".#.", "#..", "#.."],
        ['>'] = ["#..", ".#.", "..#", ".#.", "#.."],
        ['%'] = ["#.#", "..#", ".#.", "#..", "#.#"],
        ['@'] = ["###", "#.#", "###", "#..", "###"],
        ['*'] = ["#.#", ".#.", "###", ".#.", "#.#"],
        [' '] = ["...", "...", "...", "...", "..."],
    };

    public static int MeasureWidth(string text, int scale) =>
        text.Length == 0 ? 0 : (text.Length * (GlyphWidth + 1) - 1) * scale;

    public static int LineHeight(int scale) => GlyphHeight * scale;

    public static void Draw(SdlPlatform platform, string text, int x, int y, int scale, Color color)
    {
        var cursorX = x;
        foreach (var raw in text)
        {
            var c = char.ToUpperInvariant(raw);
            if (Glyphs.TryGetValue(c, out var rows))
            {
                for (var row = 0; row < GlyphHeight; row++)
                {
                    for (var col = 0; col < GlyphWidth; col++)
                    {
                        if (rows[row][col] == '#')
                        {
                            platform.FillRect(cursorX + col * scale, y + row * scale, scale, scale, color);
                        }
                    }
                }
            }

            cursorX += (GlyphWidth + 1) * scale;
        }
    }

    public static void DrawCentered(SdlPlatform platform, string text, int containerWidth, int y, int scale, Color color)
    {
        var x = (containerWidth - MeasureWidth(text, scale)) / 2;
        Draw(platform, text, x, y, scale, color);
    }

    private static readonly (int Dx, int Dy)[] OutlineOffsets =
    [
        (-1, -1), (0, -1), (1, -1),
        (-1, 0), (1, 0),
        (-1, 1), (0, 1), (1, 1),
    ];

    public static void DrawOutlined(SdlPlatform platform, string text, int x, int y, int scale, Color fill, Color outline)
    {
        foreach (var (dx, dy) in OutlineOffsets)
        {
            Draw(platform, text, x + dx * scale, y + dy * scale, scale, outline);
        }

        Draw(platform, text, x, y, scale, fill);
    }

    public static void DrawCenteredOutlined(
        SdlPlatform platform, string text, int containerWidth, int y, int scale, Color fill, Color outline)
    {
        var x = (containerWidth - MeasureWidth(text, scale)) / 2;
        DrawOutlined(platform, text, x, y, scale, fill, outline);
    }
}
// end AI-generated
