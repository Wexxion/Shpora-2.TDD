using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class TextAnalyzer
    {
        private readonly string text;
        private readonly int minWordLength;
        private readonly int maxFontSize;
        private readonly int minFontSize;
        private readonly int topNWords;
        private readonly int delta;
        public TextAnalyzer(string text, int minWordLength = 3, int topNWords = 0, int maxFontSize = 72, int minFontSize = 20)
        {
            if(string.IsNullOrEmpty(text))
                throw new ArgumentException("String can't be null or empty!");
            this.text = text;
            this.topNWords = topNWords;
            this.maxFontSize = maxFontSize;
            this.minFontSize = minFontSize;
            this.minWordLength = minWordLength;
            delta = minFontSize * 2 / 5;
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

        public List<Word> GetWordsWithSizes()
        {
            var result = new List<Word>();
            var allWords = FindAllwords();
            var wordsCounter = topNWords == 0
                ? CountWords(allWords).ToList()
                : CountWords(allWords).Take(topNWords).ToList();
            var currentFontSize = maxFontSize;
            foreach (var wordPair in wordsCounter)
            {
                result.Add(new Word(wordPair.Word, wordPair.Count, currentFontSize));
                if (currentFontSize - delta > minFontSize) currentFontSize -= delta;
            }
            return result;
        }
    }
}
