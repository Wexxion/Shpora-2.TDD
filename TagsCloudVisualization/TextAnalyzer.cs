using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    class TextAnalyzer
    {
        private string Text { get; }
        private int MinWordLength { get; }

        //для чего это поле публичное? По сути оно нужно только для использования внутри класса.
        //Тесты не причина показывать кишки.
        public List<string> Words { get; private set; }
        public List<(string Word, int Count)> WordsCounter { get; private set; }
        public TextAnalyzer(string text, int minWordLength = 3)
        {
            if(string.IsNullOrEmpty(text))
                throw new ArgumentException("String can't be null or empty!");
            Text = text;
            MinWordLength = minWordLength;
        }

        public TextAnalyzer FindAllwords()
        {
            //посмотри на другие методы получения слов по регвыру. Такие, чтобы код не марался кастами, например
            var matches = Regex.Matches(Text, @"\b[\w']*\b");
            Words = matches.Cast<Match>()
                .Select(x => x.Value.ToLower())
                .Where(y => !string.IsNullOrEmpty(y) && y.Length > MinWordLength)
                .ToList();
            return this;
        }

        public TextAnalyzer CountWords()
        {
            if (Words == null) throw new InvalidOperationException("You need to find all words first!");
            WordsCounter = Words
                .GroupBy(x => x)
                .Select(y => (y.Key, y.Count()))
                .OrderByDescending(z => z.Item2)
                .ToList();
            return this;
        }
    }
}
