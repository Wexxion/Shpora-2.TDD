using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BowlingGame
{

    public class Frame
    {
        public Frame Previous;
        public Frame Next;
        public int?[] Tries = {null, null};
        public int Bonus;
        public int Score => (int)Tries.Sum() + Bonus;
        public bool IsFinished => (Tries[0] != null && Tries[1] != null) || Tries.Sum() == 10;
        public bool IsSpare => Tries[0] != null && Tries[1] != null && Tries.Sum() == 10;
        public bool IsStrike => Tries[0] == 10;

        public void AddRollResult(int pins, bool isSpare = false)
        {
            if (pins > 10)
                throw new ArgumentException("Pins number cannot be more than 10");
            if (Tries[0] == null)
            {
                Tries[0] = pins;
                if (Previous != null && Previous.IsSpare)
                    Bonus = pins;
                if (Previous != null && Previous.IsStrike)
                {
                    Bonus += pins;
                    if (Previous.Previous != null && Previous.Previous.IsStrike)
                        Bonus += pins;
                }
                    
            }
                
            else
            {
                if (Tries[0] + pins > 10)
                    throw new ArgumentException("Frame sum cannot be more than 10");
                if (Previous != null && Previous.IsStrike)
                {
                    Bonus += pins;
                    if (Previous.Previous != null && Previous.Previous.IsStrike)
                        Bonus += pins;
                }
                    
                Tries[1] = pins;
            }
                
            
        }
    }
	public class Game
	{
	    private Frame First;
	    private Frame Current => GetFrames().Last();
	    private bool IsSpare;
	    public int FrameCount => GetFrames().Count(item => item.Tries[0] != null);

	    public Game() => First = new Frame();

	    private IEnumerable<Frame> GetFrames()
	    {
	        var current = First;
	        while (current != null)
	        {
	            yield return current;
	            current = current.Next;
	        }
	    }

		public void Roll(int pins)
		{
		    Current.AddRollResult(pins);
		    if (Current.IsFinished)
		    {
                var newFrame = new Frame();
		        var current = Current;
		        current.Next = newFrame;
		        newFrame.Previous = current;
		    }
        }
	    public int GetScore() => GetFrames().Sum(item => item.Score);
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "3 Lifanov Kryachko"; // Ivanov Petrov

	    private Game game;

	    [SetUp]
	    public void SetUp()
	    {
	        game = new Game();
	    }

		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			new Game()
				.GetScore()
				.Should().Be(0);
		}

	    [Test]
	    public void SixPins_ScoreIsSix()
	    {
            game.Roll(6);
	        game.GetScore().Should().Be(6);
	    }

	    [Test]
	    public void Should_Return_20_when_Spare_6_4_and_5()
	    {
	        game.Roll(6);
	        game.Roll(4);
	        game.Roll(5);
	        game.GetScore().Should().Be(20);
        }

	    [Test]
	    public void Should_Throw_when_ScoreInFrame_MoreThan_10()
	    {
	        Action act = () => game.Roll(12);
            act.ShouldThrow<ArgumentException>().WithMessage("Pins number*");
	    }

	    [Test]
	    public void Should_Throw_when_FrameSum_IsMoreThan_10()
	    {
	        game.Roll(8);
	        Action act = () => game.Roll(4);
	        act.ShouldThrow<ArgumentException>("Frame sum*");
        }

	    [Test]
	    public void Should_return_23_when_roll_6_4_5_3()
	    {
	        game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(3);
	        game.GetScore().Should().Be(23);
	    }

	    [TestCase(new [] {1, 2}, ExpectedResult = 1, TestName = "Simple")]
	    [TestCase(new [] {1, 2, 3, 4, 5}, ExpectedResult = 3, TestName ="Half of frame")]
	    [TestCase(new [] {1, 2, 10, 3}, ExpectedResult = 3, TestName = "Strike")]
	    [TestCase(new int[] {}, ExpectedResult = 0, TestName = "Empty")]
	    public int Should_CountFrames_Correctly(params int[] rolls)
	    {
	        foreach (var roll in rolls)
	            game.Roll(roll);
	        return game.FrameCount;
	    }

	    [Test]
	    public void Should_return_28_when_roll_10_6_3()
        {
            game.Roll(10);
            game.Roll(6);
            game.Roll(3);
            game.GetScore().Should().Be(28);
        }

	    [Test]
	    public void Should_return_39_when_roll_10_10_3()
	    {
	        game.Roll(10);
	        game.Roll(10);
	        game.Roll(3);
	        game.GetScore().Should().Be(39);
        }

	    [Test]
	    public void Should_have_correct_result_after_3_strikes()
	    {
	        game.Roll(10);
	        game.Roll(10);
	        game.Roll(10);
	        game.GetScore().Should().Be(60);
        }

	    [Test]
	    public void Strike_and_spair()
	    {
	        game.Roll(10);
	        game.Roll(10);
	        game.Roll(6);
	        game.Roll(4);
            game.Roll(10);
	        game.GetScore().Should().Be(80);
        }
	}
}
