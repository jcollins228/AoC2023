using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4Part1
    {
        public class Card
        {
            public int Number { get; set; } = 0;
            public List<int>? Pool { get; set; }
            public List<int>? PlayedNumbers { get; set; }
            public List<int>? Correct { get; set; }
            public int Score { get; set; } = 0;
        }

        public static void Day4Part1_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(4, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(4, "InputData.txt");

            var allCards = new List<Card>();
            foreach (var currentCard in inputData)
            {
                var i = currentCard.IndexOf(':');
                var cardNum = currentCard.Substring(0, i).Trim();
                cardNum = cardNum.Replace("Card ", "").Trim();

                var newCard = new Card() { Number = int.Parse(cardNum) };

                var allNumbers = currentCard.Substring(i, currentCard.Length - i).Replace(":", "").Trim();
                var numberSplit = allNumbers.Split("|");
                var poolList = numberSplit[0].Trim().Split(" ").ToList();
                poolList = poolList.Where(x => x != "").ToList();
                var playedList = numberSplit[1].Trim().Split(" ").ToList();
                playedList = playedList.Where(x => x != "").ToList();

                newCard.Pool = poolList.Select(x => int.Parse(x)).ToList();
                newCard.PlayedNumbers = playedList.Select(x => int.Parse(x)).ToList();

                newCard.Correct = newCard.PlayedNumbers.Where(x => newCard.Pool.Contains(x)).ToList();

                if (newCard.Correct.Count > 0)
                {
                    newCard.Score = 1;
                    if (newCard.Correct.Count > 1)
                    {
                        var c = 1;
                        while (c < newCard.Correct.Count)
                        {
                            newCard.Score = newCard.Score * 2;
                            c++;
                        }
                    }
                }

                allCards.Add(newCard);
            }

            var sum = allCards.Sum(x => x.Score);
            Console.WriteLine(sum);
        }
    }
}
