using System;
using System.Collections.Generic;

namespace PiranhaEmulator.Modes.TestMode;

public class TestMode : PiranhaMode
{
    private TestModeState state = TestModeState.GlyphInscriptions;

    public TestMode(MainWindow piranhaWindow) : base(piranhaWindow)
    {
    }

    public override void Init()
    {
        UpdateScreen();
    }

    public override void OnButtonClicked(PiranhaButton button)
    {
        if (button != PiranhaButton.Right && button != PiranhaButton.Left) return;
        switch (state)
        {
            case TestModeState.GlyphInscriptions:
                state = TestModeState.CustomInscriptions;
                break;
            case TestModeState.CustomInscriptions:
                state = TestModeState.GlyphInscriptions;
                break;
        }

        UpdateScreen();
    }

    protected override void OnEffectIntensityChanged()
    {
        
    }

    private void UpdateScreen()
    {
        piranhaWindow.Clear();
        switch (state)
        {
            case TestModeState.GlyphInscriptions:
                DrawAllGlyphs();
                break;
            case TestModeState.CustomInscriptions:
                DrawAllCustom();
                break;
        }
    }

    private void DrawAllGlyphs()
    {
        List<char> chars = piranhaWindow.font.GetAvailableChars();
        string s = "";
        int charIndex = 0;
        for (charIndex = 0; charIndex < chars.Count; charIndex++)
        {
            if (charIndex % 19 == 0 && charIndex != 0)
            {
                piranhaWindow.DrawString(5, 5 + 8 * (charIndex / 19), s);
                s = "" + chars[charIndex];
            }
            else s += chars[charIndex];
        }

        if (s != "")
            piranhaWindow.DrawString(10, 5 + 8 * (charIndex / 19 + 1), s);
    }

    private void DrawAllCustom()
    {
        List<string> custom = piranhaWindow.font.GetAvailableCustomInscriptions();
        int currentWidth = 5, i = 0, maxHeight = 0, currentY = 5;
        for (i = 0; i < custom.Count; i++)
        {
            List<List<bool>>? customInscription = piranhaWindow.font.GetCustomInscription(custom[i]);
            if (customInscription == null) continue;
            if (currentWidth + customInscription[0].Count > MainWindow.SCREEN_WIDTH)
            {
                currentY += maxHeight;
                piranhaWindow.DrawCustomCharacter(5, currentY, custom[i]);
                currentWidth = 5 + customInscription[0].Count;
                maxHeight = 0;
            }
            else
            {
                if (customInscription.Count > maxHeight) maxHeight = customInscription.Count;
                piranhaWindow.DrawCustomCharacter(currentWidth, currentY, custom[i]);
                currentWidth += customInscription[0].Count + 1;
            }
        }
    }
}