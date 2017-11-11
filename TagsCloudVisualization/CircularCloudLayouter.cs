using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles;
        //тут прото на подумать: коллекция теперь ридонли, но с элементами коллекции ты можешь делать что угодно (изменять размеры, например). Что можно с этим поделать?
        public IReadOnlyCollection<Rectangle> Rectangles => rectangles.AsReadOnly();
        public readonly Point Center;
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Center with negative coordinates is not allowed!");
            Center = center;
            rectangles = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var point = spiral.GetNextPoint();
                var rectangle = new Rectangle(point, rectangleSize);
                
                if (rectangle.IntersectsWith(rectangles))
                    continue;
                
                rectangles.Add(rectangle);
                return rectangle;
            }
        }
    }
}
