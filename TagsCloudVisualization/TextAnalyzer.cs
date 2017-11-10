using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    class TextAnalyzer
    {
        private readonly string text;
        private readonly int minWordLength;
        private readonly int maxFontSize;
        private readonly int minFontSize;
        private readonly int topNWords;
        public TextAnalyzer(string text, int topNWords = 0, int minWordLength = 3, int maxFontSize = 96, int minFontSize = 24)
        {
            if(string.IsNullOrEmpty(text))
                throw new ArgumentException("String can't be null or empty!");
            this.text = text;
            this.topNWords = topNWords;
            this.maxFontSize = maxFontSize;
            this.minFontSize = minFontSize;
            this.minWordLength = minWordLength;
        }

        private IEnumerable<string> FindAllwords()
        {
            var delims = new[] { '.', ',', ';', ' ', '\n', '?', '!', ':', '(', ')', '[', ']', '{', '}', '\'', '"', '–' };
            return text.Split(delims, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .Where(y => y.Length > minWordLength);
        }

        private IEnumerable<(string Word, int Count)> CountWords(IEnumerable<string> words)
        {
            return words
                .GroupBy(x => x)
                .Select(y => (y.Key, y.Count()))
                .OrderByDescending(z => z.Item2);
        }

        public IEnumerable<Word> GetWordsWithSizes()
        {
            var allWords = FindAllwords();
            var wordsCounter = topNWords == 0 ? 
                CountWords(allWords).ToList() : CountWords(allWords).Take(topNWords).ToList();
            var maxCount = wordsCounter.First().Count;
            foreach (var wordPair in wordsCounter)
            {
                var fontSize = Math.Max(minFontSize, maxFontSize * wordPair.Count / maxCount);
                yield return new Word(wordPair.Word, wordPair.Count, fontSize);
            }
        }
    }
}
