using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Geometry
    {
        public static Size CalculateImageSize(IReadOnlyCollection<Rectangle> rectangles)
        {
            var minX = rectangles.Min(rect => rect.X);
            var minY = rectangles.Min(rect => rect.Y);
            var maxX = rectangles.Max(rect => rect.X + rect.Width);
            var maxY = rectangles.Max(rect => rect.Y + rect.Height);
            var maxWidth = rectangles.Max(rect => rect.Width);
            var maxHeight = rectangles.Max(rect => rect.Height);
            var width = maxX - minX + maxWidth;
            var height = maxY - minY + maxHeight;
            return new Size(width, height);
        }
        public static List<Rectangle> TransopisteRectsCoordinates(List<Rectangle> rectangles, Point newCenter, Point oldCenter)
        {
            var dx = newCenter.X - oldCenter.X;
            var dy = newCenter.Y - oldCenter.Y;

            var newRectangles = new List<Rectangle>();
            foreach (var rectangle in rectangles)
            {
                var location = rectangle.Location;
                location.Offset(dx, dy);
                var newRectangle = new Rectangle(location, rectangle.Size);
                newRectangles.Add(newRectangle);
            }
            return newRectangles;
        }

        public static Point GetNewCenter(List<Rectangle> rectangles, Point oldCenter)
        {
            var newX = oldCenter.X;
            var newY = oldCenter.Y;

            var minX = rectangles.Min(rect => rect.X);
            var minY = rectangles.Min(rect => rect.Y);
            var maxX = rectangles.Max(rect => rect.X);
            var maxY = rectangles.Min(rect => rect.Y);


            if (minX < 0)
                newX = oldCenter.X - minX;
            if (minY < 0)
                newY = oldCenter.Y - minY;

            return new Point(newX, newY);
        }
    }
}
