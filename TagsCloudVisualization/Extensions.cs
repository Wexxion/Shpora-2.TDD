using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Extensions
    {
        public static bool IntersectsWith(this Rectangle rect, List<Rectangle> rectangles)
        {
            for (var i = rectangles.Count - 1; i >= 0; i--)
                if (rect.IntersectsWith(rectangles[i]))
                    return true;
            return false;
        }

        public static List<T> ReferenceClone<T>(this IReadOnlyCollection<T> listToClone) 
            => listToClone.Select(item => item).ToList();


        public static IEnumerable<Size> GenerateRandomRectSize(int count = 1)
        {
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                yield return new Size(rnd.Next(35, 75), rnd.Next(15, 50));
        }
    }
}
