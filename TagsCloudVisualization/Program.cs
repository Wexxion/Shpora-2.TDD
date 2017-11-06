using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            DrawRandomRects(2000, "Rect2000");
            //DrawWordCloud(@"..\..\VisualizationData\Hero of our Time.txt", 200, 5);
        }

        public static void DrawWordCloud(string source, int topNWords, int minWordLength)
        {
            var textLines = File.ReadAllLines(source);
            var text = textLines.Aggregate((i, j) => i + "\n" + j);
            var analyzer = new TextAnalyzer(text, minWordLength);
            analyzer
                .FindAllwords()
                .CountWords();
            var words = analyzer.WordsCounter.Take(topNWords).ToList();
            var viz = new TagCloudVizualizer(@"..\..\VisualizationData\WordCloud.png");
            viz.DrawTagCloud(words);
        }

        public static void DrawRandomRects(int count, string name)
        {
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            foreach (var size in GenerateRandomRectSize(count))
                layouter.PutNextRectangle(size);
            var viz = new TagCloudVizualizer($@"..\..\VisualizationData\{name}.png");
            viz.DrawRectCloud(layouter.Rectangles, center);
        }

        public static IEnumerable<Size> GenerateRandomRectSize(int count = 1)
        {
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                yield return new Size(rnd.Next(35, 75), rnd.Next(15, 50));
        }
    }
}
