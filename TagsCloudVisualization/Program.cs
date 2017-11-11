using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                args = new[] {"Words", @"..\..\VisualizationData\Hero of our Time.txt", "3", "50"};
            }

            switch (args[0])
            {
                case "Rect":
                {
                    var n = int.Parse(args[1]);
                    DrawRandomRects(n, $"Rect{n}");
                    break;
                }
                case "Words":
                {
                    var path = args[1];
                    switch (args.Length)
                    {
                        case 3:
                            DrawWordCloud(path, minWordLength: int.Parse(args[2]));
                            break;
                        case 4:
                            DrawWordCloud(path, minWordLength: int.Parse(args[2]), topNWords: int.Parse(args[3]));
                            break;
                        default:
                            DrawWordCloud(path);
                            break;
                    }
                    break;
                }
                default:
                    PrintUsage();
                    break;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\tTo draw Rects: [Rect N] where:");
            Console.WriteLine("\t\t N - number of rects. This is standart value for program");
            Console.WriteLine("\tTo draw WordCloud: [Words PATH [M] [N]] where:");
            Console.WriteLine("\t\t PATH - path to file with text");
            Console.WriteLine("\t\t M - min word length, default is 3");
            Console.WriteLine("\t\t N - topNwords, 0 for all");
        }

        public static void DrawWordCloud(string source, int minWordLength = 3, int topNWords = 0)
        {
            var text = File.ReadAllText(source);
            var analyzer = new TextAnalyzer(text, minWordLength, topNWords);
            var viz = new TagCloudVizualizer(@"..\..\VisualizationData\WordCloud.png");
            var words = analyzer.GetWordsWithSizes();
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            foreach (var word in words)
                word.LayoutRectangle = layouter.PutNextRectangle(word.Size);
            viz.DrawTagCloud(words, center);
        }

        public static void DrawRandomRects(int count, string name)
        {
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            foreach (var size in Extensions.GenerateRandomRectSize(count))
                layouter.PutNextRectangle(size);
            var viz = new TagCloudVizualizer($@"..\..\VisualizationData\{name}.png");
            viz.DrawRectCloud(layouter.Rectangles, center);
        }
    }
}