using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        //хоть поле и readonly, можно изи изменять содержимое листа
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();
        public readonly Point Center;
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Center with negative coordinates is not allowed!");
            Center = center;
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var point = spiral.NextPoint;
                var rectangle = new Rectangle(point, rectangleSize);
                
                if (rectangle.IntersectsWith(Rectangles))
                    continue;
                
                Rectangles.Add(rectangle);
                return rectangle;
            }
        }
    }

    public static class RectangleExtesions
    {
        public static bool IntersectsWith(this Rectangle rect, List<Rectangle> rectangles)
        {
            for (var i = rectangles.Count - 1; i >= 0; i--)
                if (rect.IntersectsWith(rectangles[i]))
                    return true;
            return false;
        }
    }
}
