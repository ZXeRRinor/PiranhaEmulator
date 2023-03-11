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

namespace PiranhaEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PIXEL_SIZE = 2;
        private const int SCREEN_WIDTH = 125;
        private const int SCREEN_HEIGHT = 64;
        private readonly Color PIXEL_COLOR_DARK = Color.FromRgb(69, 83, 66);
        private readonly Color PIXEL_COLOR_LIGHT = Color.FromRgb(99, 138, 107);
        private Rectangle[,] pixels = new Rectangle[SCREEN_HEIGHT, SCREEN_WIDTH];

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < SCREEN_WIDTH; i++)
            for (int j = 0; j < SCREEN_HEIGHT; j++)
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
                Canvas.SetLeft(rect, i * PIXEL_SIZE);
                Canvas.SetTop(rect, j * PIXEL_SIZE);
                pixels[j, i] = rect;
                canvasDisplay.Children.Add(rect);
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

            if (result.VisualHit is Ellipse ellipse)
            {
                buttonName = ellipse.Name;
                Debug.WriteLine($"Hit {ellipse.Name}");
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