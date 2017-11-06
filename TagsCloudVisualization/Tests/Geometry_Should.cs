using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class Geometry_Should
    {
        [TestCaseSource("TestCasesForImageSizeTest")]
        public Size ReturnCorrectSize(List<Rectangle> rectangles)
        {
            return Geometry.CalculateImageSize(rectangles);
        }

        private static IEnumerable TestCasesForImageSizeTest()
        {
            return new[]
            {
                new TestCaseData(new List<Rectangle>
                        {
                            new Rectangle(Point.Empty, new Size(10, 10)),
                            new Rectangle(new Point(-100, -100), new Size(10, 10))
                        })
                    .Returns(new Size(120, 120))
                    .SetName("Distanse between rects is 100X and 100Y + width and height 10 => should be 120x120"),

                new TestCaseData(new List<Rectangle>
                        {
                            new Rectangle(Point.Empty, new Size(10, 10)),
                            new Rectangle(new Point(-100, -100), new Size(10, 10)),
                            new Rectangle(new Point(100, 100), new Size(10, 10))
                        })
                    .Returns(new Size(220, 220))
                    .SetName("More complicated test"),
            };
        }


        [TestCaseSource("TestCasesForNewCenterTest")]
        public Point CorrectlyFindNewCenter(List<Rectangle> rectangles, Point center)
        {
            return Geometry.GetNewCenter(rectangles, center);
        }

        private static IEnumerable TestCasesForNewCenterTest()
        {
            return new[]
            {
                new TestCaseData(new List<Rectangle>
                    {
                        new Rectangle(Point.Empty, new Size(100, 100)),
                        new Rectangle(new Point(-100, -100), new Size(100, 100))
                    },
                     Point.Empty)
                    .Returns(new Point(100, 100))
                    .SetName("Simple test, negative X and Y"),
                new TestCaseData(new List<Rectangle>
                    {
                        new Rectangle(Point.Empty, new Size(100, 100)),
                        new Rectangle(new Point(-100, 0), new Size(100, 100))
                    },
                    Point.Empty)
                    .Returns(new Point(100, 0))
                    .SetName("Simple test, negative X"),
                new TestCaseData(new List<Rectangle>
                    {
                        new Rectangle(Point.Empty, new Size(100, 100)),
                        new Rectangle(new Point(0, -100), new Size(100, 100))
                    },
                    Point.Empty)
                    .Returns(new Point(0, 100))
                    .SetName("Simple test, negative Y"),
                new TestCaseData(new List<Rectangle>
                    {
                        new Rectangle(new Point(100, 100), new Size(100, 100)),
                        new Rectangle(new Point(0, 0), new Size(100, 100))
                    },
                    new Point(100, 100))
                    .Returns(new Point(100, 100))
                    .SetName("All coordinates are positive")
            };
        }

        [TestCaseSource("TestCasesForTranspositionTest")]
        public List<Rectangle> CorrectluTranspositeCoordinatesOfAllRectangles(List<Rectangle> rectangles, Point newCenter, Point oldCenter)
        {
            return Geometry.TransopisteRectsCoordinates(rectangles, newCenter, oldCenter);
        }

        private static IEnumerable TestCasesForTranspositionTest()
        {
            return new[]
            {
                new TestCaseData(new List<Rectangle> { new Rectangle(Point.Empty, new Size(100, 100))}, 
                    new Point(100, 100), Point.Empty)
                    .Returns(new List<Rectangle> { new Rectangle(new Point(100, 100), new Size(100, 100))})
                    .SetName("New center (100, 100), old (0, 0)"),

                new TestCaseData(new List<Rectangle> { new Rectangle(new Point(100, 100), new Size(100, 100))},
                        new Point(0, 0), new Point(100, 100))
                    .Returns(new List<Rectangle> { new Rectangle(new Point(0, 0), new Size(100, 100))})
                    .SetName("New center (0, 0), old (100, 100)"),

                new TestCaseData(new List<Rectangle>
                        {
                            new Rectangle(new Point(50, 50), new Size(100, 100)),
                            new Rectangle(new Point(100, 100), new Size(100, 100))
                        },
                        new Point(100, 100), new Point(50, 50))
                    .Returns(new List<Rectangle>
                        {
                            new Rectangle(new Point(100, 100), new Size(100, 100)),
                            new Rectangle(new Point(150, 150), new Size(100, 100))
                        })
                    .SetName("2 rects, New center (100, 100), old (50, 50)")
            };
        }
    }
}
