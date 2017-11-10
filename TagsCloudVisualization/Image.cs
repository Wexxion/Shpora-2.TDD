using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class Image
    {
        private readonly string filepath;
        private Bitmap bitmap;

        public Image(string filepath) => this.filepath = filepath;

        public Graphics Configure(IReadOnlyCollection<Rectangle> rectangles, Point center)
        {

            var maxWidth = rectangles.Max(rect => rect.Width);
            var maxHeight = rectangles.Max(rect => rect.Height);
            var size = Geometry.CalculateImageSize(rectangles);

            bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);

            var dx = size.Width / 2 - center.X - maxWidth / 2;
            var dy = size.Height / 2 - center.Y - maxHeight / 2;
            graphics.TranslateTransform(dx, dy);

            graphics.Clear(Color.White);

            return graphics;
        }

        public void Save() => bitmap.Save(filepath);
    }
}
