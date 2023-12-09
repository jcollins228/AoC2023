﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day7Part1
    {
        public static Dictionary<char, int> CardStrength { get; set; } = new Dictionary<char, int>()
        {
            { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 },
            { '9', 9 }, { 'T', 10 }, { 'J', 11 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 }
        };

        public class Card
        {
            public char Face { get; set; }
            public int Value { get; set; }

            public Card(char face)
            {
                Face = face;
                Value = CardStrength[face];
            }
        }

        public class Hand : IComparable<Hand>
        {
            public string OriginalOrder { get; set; }
            public int Bid { get; set; }
            public int InitialRank { get; set; }
            public int FinalRank { get; set; }
            public List<Card> HandCards { get; set; }
            public Dictionary<char, List<Card>> Duplicates { get; set; }

            public Hand(string originalOrder, int bid)
            {
                OriginalOrder = originalOrder;
                Bid = bid;
                InitialRank = 0;
                FinalRank = 0;
                Duplicates = new Dictionary<char, List<Card>>();
                var faces = originalOrder.ToCharArray();
                HandCards = new List<Card>();
                foreach (var indFace in faces)
                {
                    HandCards.Add(new Card(indFace));
                }
                FindDuplicates();
            }

            public void FindDuplicates()
            {
                var cardData = OriginalOrder.ToCharArray();
                foreach (var i in cardData)
                {
                    var newCard = new Card(i);
                    if (Duplicates.ContainsKey(newCard.Face))
                        Duplicates[newCard.Face].Add(newCard);
                    else
                        Duplicates.Add(newCard.Face, new List<Card> { newCard });
                }

                var uniqueDupes = Duplicates.Keys.Count;
                var onePair = Duplicates.Where(x => x.Value.Count == 2).Count();
                var threeOfAKind = Duplicates.Where(x => x.Value.Count == 3).Count();
                var fourOfAKind = Duplicates.Where(x => x.Value.Count == 4).Count();
                var fiveOfAKind = Duplicates.Where(x => x.Value.Count == 5).Count();

                // high card
                if (uniqueDupes == 5)
                    InitialRank = 0;
                // one pair
                else if (uniqueDupes == 4 && onePair == 1)
                    InitialRank = 1;
                // two pair
                else if (uniqueDupes == 3 && onePair == 2)
                    InitialRank = 3;
                // three of a kind
                else if (uniqueDupes == 3 && threeOfAKind == 1)
                    InitialRank = 4;
                // full house
                else if (uniqueDupes == 2 && threeOfAKind == 1 && onePair == 1)
                    InitialRank = 5;
                // four of a kind
                else if (uniqueDupes == 2 && fourOfAKind == 1)
                    InitialRank = 6;
                // five of a kind
                else if (uniqueDupes == 1 && fiveOfAKind == 1)
                    InitialRank = 7;
            }

            public int CompareTo(Hand? comparedHand)
            {
                if (comparedHand == null)
                    return 0;
                if (this.HandCards == comparedHand.HandCards)
                    return 0;
                if (this.HandCards[0].Value < comparedHand.HandCards[0].Value)
                    return -1;
                if (this.HandCards[0].Value > comparedHand.HandCards[0].Value)
                    return 1;
                if (this.HandCards[1].Value < comparedHand.HandCards[1].Value)
                    return -1;
                if (this.HandCards[1].Value > comparedHand.HandCards[1].Value)
                    return 1;
                if (this.HandCards[2].Value < comparedHand.HandCards[2].Value)
                    return -1;
                if (this.HandCards[2].Value > comparedHand.HandCards[2].Value)
                    return 1;
                if (this.HandCards[3].Value < comparedHand.HandCards[3].Value)
                    return -1;
                if (this.HandCards[3].Value > comparedHand.HandCards[3].Value)
                    return 1;
                if (this.HandCards[4].Value < comparedHand.HandCards[4].Value)
                    return -1;
                if (this.HandCards[4].Value > comparedHand.HandCards[4].Value)
                    return 1;
                else
                    return -1;
            }

        }

        public static void Day7Part1_Main(string[] args)
        {

            //var inputData = Utilities.GetFileData(7, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(7, "InputData.txt");

            var allHands = new List<Hand>();

            foreach (var indHand in inputData)
            {
                var handData = indHand.Split(' ');
                var newHand = new Hand(handData[0], int.Parse(handData[1]));
                allHands.Add(newHand);
            }
            allHands = allHands.OrderBy(x => x.InitialRank).ToList();

            var sortedHands = new List<Hand>();
            var finalRank = 1;

            for (var i = 0; i <= 7; i++)
            {
                var initialRankHands = allHands.Where(x => x.InitialRank == i).ToList();
                initialRankHands = initialRankHands.OrderBy(x => x).ToList();

                foreach (var indHand in initialRankHands)
                {
                    indHand.FinalRank = finalRank;
                    finalRank++;
                    sortedHands.Add(indHand);
                }
            }

            var multiplied = sortedHands.Select(x => x.FinalRank * x.Bid).ToList();
            var answer = multiplied.Sum();

            Console.WriteLine(answer);
        }
    }
}
