using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PiranhaEmulator.Font;
using PiranhaEmulator.Modes;
using PiranhaEmulator.Modes.TestMode;

namespace PiranhaEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int PIXEL_SIZE = 5;
        public const int PIXEL_OFFSET = PIXEL_SIZE / 5;
        public const int SCREEN_WIDTH = 125;
        public const int SCREEN_HEIGHT = 64;

        private readonly Color PIXEL_COLOR_DARK = Color.FromRgb(69, 83, 66);
        private readonly Color PIXEL_COLOR_LIGHT = Color.FromRgb(99, 138, 107);
        private Rectangle[,] pixels = new Rectangle[SCREEN_HEIGHT, SCREEN_WIDTH];
        private PiranhaMode? currentMode = null;
        private bool[] modeArray = new bool[] { true, false, false };

        public int SelectedMode => Array.IndexOf(modeArray, true);
        public bool[] ModeArray => modeArray;
        public readonly PiranhaFont font = new PiranhaFont();

        public MainWindow()
        {
            InitializeComponent();
            for (int line = 0; line < SCREEN_HEIGHT; line++)
            for (int column = 0; column < SCREEN_WIDTH; column++)
            {
                var rect = new Rectangle
                {
                    Height = PIXEL_SIZE,
                    Width = PIXEL_SIZE,
                    StrokeThickness = 0,
                    Fill = new SolidColorBrush()
                    {
                        Color = PIXEL_COLOR_LIGHT
                    }
                };
                Canvas.SetLeft(rect, column * (PIXEL_SIZE + PIXEL_OFFSET));
                Canvas.SetTop(rect, line * (PIXEL_SIZE + PIXEL_OFFSET));
                pixels[line, column] = rect;
                canvasDisplay.Children.Add(rect);
            }
        }

        public void SetPixel(int x, int y, bool state)
        {
            if (x < 0 || x >= SCREEN_WIDTH || y < 0 || y >= SCREEN_HEIGHT) return;
            if (state)
                pixels[y, x].Fill = new SolidColorBrush()
                {
                    Color = PIXEL_COLOR_DARK
                };
            else
                pixels[y, x].Fill = new SolidColorBrush()
                {
                    Color = PIXEL_COLOR_LIGHT
                };
        }

        public void SetRectangle(int x1, int y1, int x2, int y2, bool state)
        {
            int leftX = x1 < x2 ? x1 : x2;
            int rightX = x1 > x2 ? x1 : x2;
            int topY = y1 < y2 ? y1 : y2;
            int bottomY = y1 > y2 ? y1 : y2;

            for (int i = leftX; i < rightX; i++)
            for (int j = topY; j < bottomY; j++)
            {
                SetPixel(i, j, state);
            }
        }

        public void Clear() => SetRectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, false);

        public void DrawCharacter(int x, int y, char c)
        {
            var glyph = font.GetGlyphInscription(c);
            if (glyph != null)
                for (int line = 0; line < PiranhaFont.GLYPH_HEIGHT; line++)
                for (int column = 0; column < PiranhaFont.GLYPH_WIDTH; column++)
                    SetPixel(x + column, y - PiranhaFont.GLYPH_HEIGHT + line, glyph[line][column]);
        }

        public void DrawCustomCharacter(int x, int y, string name)
        {
            var custom = font.GetCustomInscription(name);
            if (custom != null)
                for (int line = 0; line < custom.Count; line++)
                for (int column = 0; column < custom[0].Count; column++)
                    SetPixel(x + column, y + line, custom[line][column]);
        }

        public void DrawString(int x, int y, string s)
        {
            for (int i = 0; i < s.Length; i++)
                DrawCharacter(x + i * (PiranhaFont.GLYPH_WIDTH + PiranhaFont.GLYPH_OFFSET), y, s[i]);
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition((IInputElement)sender);
            var result = VisualTreeHelper.HitTest((Visual)sender, position);
            var buttonName = "";
            if (result.VisualHit is Rectangle rect)
            {
                buttonName = rect.Name;
                Debug.WriteLine($"Hit {rect.Name}");
            }

            if (result.VisualHit is Ellipse ellipse)
            {
                buttonName = ellipse.Name;
                Debug.WriteLine($"Hit {ellipse.Name}");
            }

            if (buttonName != "")
            {
                PiranhaButton button = PiranhaButton.None;
                switch (buttonName)
                {
                    case "button_0_Reset":
                        button = PiranhaButton.Reset0;
                        break;
                    case "button_1_Help":
                        button = PiranhaButton.Help1;
                        break;
                    case "button_2_Sa":
                        button = PiranhaButton.Sa2;
                        break;
                    case "button_3_Osc":
                        button = PiranhaButton.Osc3;
                        break;
                    case "button_4_Set":
                        button = PiranhaButton.Set4;
                        break;
                    case "button_5_Plus":
                        button = PiranhaButton.Plus5;
                        break;
                    case "button_6_Load":
                        button = PiranhaButton.Load6;
                        break;
                    case "button_7_Minus":
                        button = PiranhaButton.Minus7;
                        break;
                    case "button_8_Save":
                        button = PiranhaButton.Save8;
                        break;
                    case "button_9_Mute":
                        button = PiranhaButton.Mute9;
                        break;
                    case "button_Run_Stop":
                        button = PiranhaButton.RunStop;
                        break;
                    case "button_Enter":
                        button = PiranhaButton.Enter;
                        break;
                    case "button_Left":
                        button = PiranhaButton.Left;
                        break;
                    case "button_Down":
                        button = PiranhaButton.Down;
                        break;
                    case "button_Right":
                        button = PiranhaButton.Right;
                        break;
                    case "button_Up":
                        button = PiranhaButton.Up;
                        break;
                }

                currentMode?.OnButtonClicked(button);
            }
        }

        private void cbPower_Checked(object sender, RoutedEventArgs e)
        {
            if (cbTestMode.IsChecked == true)
            {
                currentMode = new TestMode(this);
            }
            else
            {
                switch (SelectedMode)
                {
                    case 0:
                        currentMode = new InfraredDetector(this);
                        break;
                }
            }
            cbTestMode.IsEnabled = false;
            rbModeIr.IsEnabled = false;
            rbModeRf.IsEnabled = false;
            
            currentMode?.Init();
            currentMode?.OnEffectIntensityChangedHandler(sliderEffectIntensity.Value / 10);
        }

        private void cbPower_Unchecked(object sender, RoutedEventArgs e)
        {
            Clear();
            currentMode = null;
            cbTestMode.IsEnabled = true;
            rbModeIr.IsEnabled = true;
            rbModeRf.IsEnabled = true;
        }

        private void sliderEffectIntensity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Debug.WriteLine("Outside effect intensity = " + sliderEffectIntensity.Value / 10);
            currentMode?.OnEffectIntensityChangedHandler(sliderEffectIntensity.Value / 10);
        }
    }
}