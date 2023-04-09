using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Text.Json;
using System.Windows.Media.TextFormatting;

namespace PiranhaEmulator.Font;

public class PiranhaFont
{
    // A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, X, Y, Z
    public const int GLYPH_WIDTH = 5;
    public const int GLYPH_HEIGHT = 7;
    public const int GLYPH_OFFSET = 1;

    private Dictionary<string, List<List<bool>>>? glyphs = null;

    public PiranhaFont()
    {
        glyphs = JsonSerializer.Deserialize<Dictionary<string, List<List<bool>>>>(
            File.ReadAllText("C:/Users/zx/source/repos/PiranhaEmulator/PiranhaEmulator/Font/GlyphInscriptions.json")
        );
    }

    public List<char> GetAvailableChars()
    {
        List<char> result = new List<char>();
        if (glyphs != null)
            foreach (var s in glyphs.Keys.ToList())
                if (s.Length == 1)
                    result.Add(s[0]);

        return result;
    }

    public List<List<bool>>? GetGlyphInscription(char c)
    {
        if (glyphs != null) return glyphs.ContainsKey(c.ToString()) ? glyphs[c.ToString()] : glyphs[" "];
        return null;
    }

    public List<string> GetAvailableCustomInscriptions()
    {
        List<string> result = new List<string>();
        if (glyphs != null)
            foreach (var s in glyphs.Keys.ToList())
                if (s.Length > 1)
                    result.Add(s);

        return result;
    }

    public List<List<bool>>? GetCustomInscription(string name)
    {
        if (glyphs != null) return glyphs.ContainsKey(name) ? glyphs[name] : null;
        return null;
    }
}