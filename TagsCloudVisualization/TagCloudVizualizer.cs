using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class TagCloudVizualizer
    {
        private Graphics graphics;
        private readonly Image image;
        private readonly Random rnd = new Random();

        public TagCloudVizualizer(string filepath) => image = new Image(filepath);

        public void DrawTagCloud(List<Word> words, Point center)
        {
            graphics = image.Configure(words.Select(word => word.LayoutRectangle).ToList(), center);

            foreach (var word in words)
            {
                var brush = new SolidBrush(GetRandomColor());
                graphics.DrawString(word.Value, word.Font, brush, word.LayoutRectangle);
            }

            image.Save();
        }

        public void DrawRectCloud(IReadOnlyCollection<Rectangle> rectangles, Point center)
        {
            graphics = image.Configure(rectangles, center);

            foreach (var rectangle in rectangles)
            {
                graphics.FillRectangle(new SolidBrush(GetRandomColor()), rectangle);
                graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            }

            image.Save();
        }

        private Color GetRandomColor()
        {
            var r = rnd.Next(255);
            var g = rnd.Next(255);
            var b = rnd.Next(255);
            if (r + g + b > 680) return GetRandomColor();
            var color = Color.FromArgb(r, g, b);
            return color;
        }
    }
}
