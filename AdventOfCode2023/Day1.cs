using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day1
    {
        private enum NumberWordsEnum
        {
            zero = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9
        }

        private class NumberLocation
        {
            public string? Number { get; set; }
            public int Location { get; set; }
            public int Value { get; set; }
        }

        public static void Day1Part2_Main(string[] args)
        {
            var logFile = File.ReadAllLines(@"C:\Code\AdventOfCode2023\Day 1\InputPart2.txt");
            var logList = new List<string>(logFile);
            var numberWords = new List<string>() { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var numberInts = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            var totalList = new List<int>();

            foreach (var indLine in logList)
            {
                var numberPositions = new List<NumberLocation>();
                foreach (var indNumber in numberWords)
                {
                    var allIndexes = AllIndexesOf(indLine, indNumber);
                    if (allIndexes.Count > 0)
                    {
                        var enumValue = Enum.Parse(typeof(NumberWordsEnum), indNumber);
                        int enumInt = (int)enumValue;
                        foreach (var indexLoc in allIndexes)
                        {
                            var newLoc = new NumberLocation() { Number = indNumber, Location = indexLoc, Value = enumInt };
                            numberPositions.Add(newLoc);
                        }

                    }
                }

                foreach (var indNumber in numberInts)
                {
                    var allIndexes = AllIndexesOf(indLine, indNumber);
                    if (allIndexes.Count > 0)
                    {
                        var intValue = int.Parse(indNumber);
                        foreach (var indexLoc in allIndexes)
                        {
                            var newLoc = new NumberLocation() { Number = indNumber, Location = indexLoc, Value = intValue };
                            numberPositions.Add(newLoc);
                        }
                    }
                }

                numberPositions = numberPositions.OrderBy(x => x.Location).ToList();

                var combined = numberPositions[0].Value.ToString() + numberPositions[numberPositions.Count - 1].Value.ToString();
                totalList.Add(int.Parse(combined));
            }

            int totalSum = 0;
            foreach (var indNum in totalList)
            {
                totalSum += indNum;
            }

            Console.WriteLine(totalSum);
        }

        static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static void Day1Part1_Main(string[] args)
        {
            var logFile = File.ReadAllLines(@"C:\Code\AdventOfCode2023\Day 1\InputPart1.txt");
            var logList = new List<string>(logFile);
            var totalList = new List<int>();

            foreach (var indLine in logList)
            {
                var numberList = new List<int>();
                var lineArray = indLine.ToCharArray();

                foreach (var indChar in lineArray)
                {
                    int n;

                    if (int.TryParse(indChar.ToString(), out n))
                    {
                        numberList.Add(n);
                    }
                }

                var combined = numberList[0].ToString() + numberList[numberList.Count - 1].ToString();
                totalList.Add(int.Parse(combined));
            }

            int totalSum = 0;
            foreach (var indNum in totalList)
            {
                totalSum += indNum;
            }

            Console.WriteLine(totalSum);
        }
    }
}
