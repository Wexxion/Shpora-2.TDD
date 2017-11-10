using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class TextAnalyzer_Should
    {
        [TestCase(null)]
        [TestCase("")]
        public void ThrowArgumentException_When_NullOrEmptyString(string text)
        {
            Action act = () => new TextAnalyzer(text);
            act.ShouldThrow<ArgumentException>()
                .WithMessage("*null*empty*");
        }


        [TestCase("UPPERCASE", 1, ExpectedResult = new []{"uppercase"})]
        [TestCase("Just a simple test", 3, ExpectedResult = new []{"just","simple", "test"})]
        [TestCase("Sentenses! Another? Much fun. Love testing;", 5, 
            ExpectedResult = new []{"sentenses","another","much","love", "testing"})]
        public List<string> CorrectlySplitText_And_DeleteWordsWithLenLessThan3_WhenSimpleString(string text, int count)
        {
            var textAnalyzer = new TextAnalyzer(text);
            var wordsList = textAnalyzer.GetWordsWithSizes().Select(word => word.Value).ToList();
            wordsList.Should().HaveCount(count);

            return wordsList;
        }

        private static IEnumerable TestCasesForWordsCounterTest()
        {
            return new[]
            {
                new TestCaseData("Just a simple test", 3)
                    .Returns(new List<(string, int)> {("just", 1), ("simple", 1), ("test", 1)})
                    .SetName("Each word has count = 1"),
                new TestCaseData("Test test test test", 1)
                    .Returns(new List<(string, int)> {("test", 4)})
                    .SetName("Test count = 4"),
                new TestCaseData("Test test lololo lololo ololo", 3)
                    .Returns(new List<(string, int)> {("test", 2), ("lololo", 2), ("ololo", 1)})
                    .SetName("Mixed test with 3 different words"),
                new TestCaseData("Test test test testing", 2)
                    .Returns(new List<(string, int)> {("test", 3), ("testing", 1)})
                    .SetName("Return most common word first"),
            };
        }

        [TestCaseSource("TestCasesForWordsCounterTest")]
        public List<(string Word, int Count)> CorrectlyCountWords_OnSimpleStrings(string text, int count)
        {
            var textAnalyzer = new TextAnalyzer(text);
            var words = textAnalyzer.GetWordsWithSizes().Select(word => (word.Value, word.Count)).ToList();
            words.Should().HaveCount(count);

            return words;
        }
    }
}
