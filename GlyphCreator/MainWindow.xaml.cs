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

namespace GlyphCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GLYPH_WIDTH = 5;
        private const int GLYPH_HEIGHT = 7;
        private readonly Color PIXEL_COLOR_DARK = Color.FromRgb(0, 0, 0);
        private readonly Color PIXEL_COLOR_LIGHT = Color.FromRgb(255, 255, 255);
        private Rectangle[,] pixels = new Rectangle[GLYPH_HEIGHT, GLYPH_WIDTH];

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < GLYPH_WIDTH; i++)
            for (int j = 0; j < GLYPH_HEIGHT; j++)
            {
                var rect = new Rectangle
                {
                    Name = "",
                    Height = 30,
                    Width = 30,
                    StrokeThickness = 3,
                    Fill = new SolidColorBrush()
                    {
                        Color = PIXEL_COLOR_LIGHT
                    }
                };
                Canvas.SetLeft(rect, 10 + i * 30);
                Canvas.SetTop(rect, 10 + j * 30);
                pixels[j, i] = rect;
                canvasGlyph.Children.Add(rect);
            }
        }

        public void SetPixelOnDisplay(int x, int y, bool state)
        {
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

            if (buttonName != "")
            {
                if (buttonName == "button_Enter")
                {
                    for (int i = 0; i < SCREEN_WIDTH; i++)
                    for (int j = 0; j < SCREEN_HEIGHT; j++)
                    {
                        SetPixelOnDisplay(i, j, true);
                    }
                }
            }
        }
    }
}
