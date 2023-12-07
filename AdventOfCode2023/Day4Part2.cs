using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4Part2
    {
        public class Card
        {
            public int Number { get; set; } = 0;
            public List<int>? Pool { get; set; }
            public List<int>? PlayedNumbers { get; set; }
            public List<int> Correct { get; set; } = new List<int>();
            public int Copies { get; set; } = 0;
        }

        public static void Day4Part2_Main(string[] args)
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
                allCards.Add(newCard);
            }

            for (var c = 0; c < allCards.Count; c++)
            {
                var currentCard = allCards[c];
                for (var makeCopies = 1; makeCopies <= currentCard.Correct.Count; makeCopies++)
                {
                    if (allCards.Count >= c + makeCopies)
                    {
                        var nextCard = allCards[c + makeCopies];
                        nextCard.Copies++;
                    }
                }

                for (var copyCount = 1; copyCount <= currentCard.Copies; copyCount++)
                {
                    for (var makeCopies = 1; makeCopies <= currentCard.Correct.Count; makeCopies++)
                    {
                        if (allCards.Count >= c + makeCopies)
                        {
                            var nextCard = allCards[c + makeCopies];
                            nextCard.Copies++;
                        }
                    }
                }
            }

            var totalCards = allCards.Count + allCards.Sum(x => x.Copies);
            Console.WriteLine(totalCards);
        }
    }
}
