using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using BuddyMemoryAllocation.Model;

namespace BuddyMemoryAllocation.App
{
    public class VisualizationMemory
    {
        private readonly OperationMemory _memory;
        private readonly Panel _element;
        private const int HEIGHT = 50;

        public VisualizationMemory(Panel element, OperationMemory memory)
        {
            _element = element;
            _memory = memory;
        }

        public void Refresh()
        {
            _element.Children.Clear();
            Visualization();
        }

        private delegate void Del();
        public void RefreshAsync()
        {
            Del del = Refresh;
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
              del);
        }

        private void Visualization()
        {
            var koef = (_element.ActualWidth*0.9)/_memory.Size;
            var indeptLeft = _element.ActualWidth * 0.001;
            for (var block = _memory.Map.First; block != null; block = block.Next)
            {
                var color = GetColor();
                var myRect = new Rectangle
                {
                    Stroke = Brushes.Black,
                    Fill = color,
                    Height = HEIGHT,
                    Width = block.Value.Size*koef
                };
                Canvas.SetTop(myRect, _element.ActualHeight/2 - 100);
                Canvas.SetLeft(myRect, indeptLeft);
                _element.Children.Add(myRect);
                PrintText(indeptLeft + myRect.Width/3,
                    _element.ActualHeight / 2 - 100,
                    block.Value.Size.ToString(CultureInfo.InvariantCulture));

                indeptLeft += (int)myRect.Width+5;
            }
        }

        private void PrintText(double x, double y, string text)
        {
            var textBlock = new TextBlock {Text = text};
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            _element.Children.Add(textBlock);
        }
        static Random rand = new Random();
        private Brush GetColor()
        {
            var r = (byte)rand.Next(127, 255);
            var g = (byte)rand.Next(127, 255);
            var b = (byte)rand.Next(127, 255);
            var mySolidColorBrush = new SolidColorBrush {Color = Color.FromArgb(255, r, g, b)};
            return mySolidColorBrush;
        }
    }
}
