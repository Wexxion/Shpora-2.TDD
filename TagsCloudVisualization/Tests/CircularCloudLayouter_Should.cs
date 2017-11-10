using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void PlaceFirstRetInCenter()
        {
            var size = new Size(50, 20);
            var res = layouter.PutNextRectangle(size);

            res.Location.ShouldBeEquivalentTo(center);
        }

        [TestCase(-1, -1)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void ThrowArgumentException_WhenCenterHasNegativeCoordinates(int x, int y)
        {
            var center = new Point(x, y);
            Action act = () => new CircularCloudLayouter(center);

            act.ShouldThrow<ArgumentException>().WithMessage("*negative*");
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(500)]
        public void PlaceRectangeWithGivenSize(int count)
        {
            //не круто обращаться к Program, это должна быть входная точка, не более
            var sizes = Extensions.GenerateRandomRectSize(count).ToArray();
            var rects = sizes.Select(size => layouter.PutNextRectangle(size));

            sizes.Should().Equal(rects.Select(rect => rect.Size));
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(500)]
        public void PlaceAllGivenRects(int count)
        {
            foreach (var size in Extensions.GenerateRandomRectSize(count))
                layouter.PutNextRectangle(size);

            layouter.Rectangles.Should().HaveCount(count);
        }

        [TestCase(2)]
        [TestCase(100)]
        [TestCase(500)]
        public void NotIntersectRectangles(int count)
        {
            foreach (var size in Extensions.GenerateRandomRectSize(count))
                layouter.PutNextRectangle(size);
            foreach (var rectangle in layouter.Rectangles)
            {
                var otherRects = layouter.Rectangles.ReferenceClone();
                otherRects.Remove(rectangle);

                rectangle.IntersectsWith(otherRects).Should().BeFalse();
            }
        }

        [Test]
        public void JustFailingTest()
        {
            for (int i = 0; i < 2000; i++)
                layouter.PutNextRectangle(new Size(60, 25));
            layouter.Rectangles.Should().HaveCount(2001);
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status != TestStatus.Failed) return;
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = $@"{appPath}\\..\\..\\Tests\\Fails\\{context.Test.Name}.jpg";
            var visualizer = new TagCloudVizualizer(filepath);
            visualizer.DrawRectCloud(layouter.Rectangles, layouter.Center);

            Console.WriteLine($"Tag cloud visualization saved to file {filepath}");
        }
    }
}
