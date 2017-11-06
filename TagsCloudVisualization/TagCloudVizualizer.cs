using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    class TagCloudVizualizer
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private readonly string filepath;
        private readonly Random rnd = new Random();
        private const int MaxFontSize = 96;
        private const int MinFontSize = 24;

        public TagCloudVizualizer(string filepath) => this.filepath = filepath;

        public void DrawTagCloud(List<(string Word, int Count)> words)
        {
            var wordRects = new List<(string Word, Font Font, Rectangle rect)>();
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            var maxCount = words[0].Count;

            foreach (var wordTuple in words)
            {
                var fontSize = Math.Max(MinFontSize, MaxFontSize * wordTuple.Count / maxCount);
                var font = new Font(new FontFamily("Calibri"), fontSize);
                var size = TextRenderer.MeasureText(wordTuple.Word, font);
                var rect = layouter.PutNextRectangle(size);
                wordRects.Add((wordTuple.Word, font, rect));
            }

            ConfigureImage(layouter.Rectangles, center);

            foreach (var stringData in wordRects)
            {
                var brush = new SolidBrush(GetRandomColor());
                graphics.DrawString(stringData.Word, stringData.Font, brush, stringData.rect);
            }
                
            bitmap.Save(filepath);
        }

        public void DrawRectCloud(List<Rectangle> rectangles, Point center)
        {
            ConfigureImage(rectangles, center);

            foreach (var rectangle in rectangles)
            {
                graphics.FillRectangle(new SolidBrush(GetRandomColor()), rectangle);
                graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            }

            bitmap.Save(filepath);
        }

        private void ConfigureImage(List<Rectangle> rectangles, Point center)
        {
            var maxWidth = rectangles.Max(rect => rect.Width);
            var maxHeight = rectangles.Max(rect => rect.Height);
            var size = Geometry.CalculateImageSize(rectangles);

            bitmap = new Bitmap(size.Width, size.Height);
            graphics = Graphics.FromImage(bitmap);

            var dx = size.Width / 2 - center.X - maxWidth / 2;
            var dy = size.Height / 2 - center.Y - maxHeight / 2;
            graphics.TranslateTransform(dx, dy);

            graphics.Clear(Color.White);
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
