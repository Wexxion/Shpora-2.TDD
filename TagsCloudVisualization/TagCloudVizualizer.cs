using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    class TagCloudVizualizer
    {
        private Graphics graphics;
        private readonly Image image;
        private readonly Random rnd = new Random();

        public TagCloudVizualizer(string filepath) => image = new Image(filepath);

        public void DrawTagCloud(TextAnalyzer textAnalyzer)
        {

            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            var words = textAnalyzer.GetWordsWithSizes().ToArray();

            foreach (var word in words)
                word.LayoutRectangle = layouter.PutNextRectangle(word.Size);

            graphics = image.Configure(layouter.Rectangles, center);

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
