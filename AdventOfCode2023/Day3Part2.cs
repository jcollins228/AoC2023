using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day3Part2
    {
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coordinate()
            {
                X = 0;
                Y = 0;
            }
        }

        public class CharLoc
        {
            public string Number { get; set; }
            public Coordinate Start { get; set; }
            public Coordinate End { get; set; }

            public CharLoc()
            {
                Number = string.Empty;
                Start = new Coordinate();
                End = new Coordinate();
            }
        }

        public static string? CurrentNumber { get; set; }

        static void Day3Part2_Main(string[] args)
        {
            //Day3Part1.Day3Part1_Main(null);

            //var inputData = Utilities.GetFileData(3, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(3, "InputData.txt");

            var y = 0;
            var numbers = new List<CharLoc>();
            var stars = new List<CharLoc>();

            while (y < inputData.Count)
            {
                var charList = inputData[y].ToCharArray();
                var x = 0;
                while (x < charList.Length)
                {
                    if (char.IsNumber(charList[x]))
                    {
                        var newNumber = new CharLoc();
                        newNumber.Start.X = x;
                        newNumber.Start.Y = y;

                        CurrentNumber = charList[x].ToString();
                        CheckNext(charList, x);
                        newNumber.Number = CurrentNumber;
                        x = x + CurrentNumber.Length;
                        newNumber.End.X = x - 1;
                        newNumber.End.Y = y;

                        numbers.Add(newNumber);
                    }
                    else if (charList[x] == '*')
                    {
                        var starLoc = new CharLoc() { Number = "*", Start = new Coordinate() { X = x, Y = y } };
                        stars.Add(starLoc);
                        x++;
                    }
                    else
                    {
                        x++;
                    }
                }
                y++;
            }

            var results = new List<int>();
            foreach (var currentStar in stars)
            {
                var xRange = Enumerable.Range(currentStar.Start.X - 1, 3);
                var yRange = Enumerable.Range(currentStar.Start.Y - 1, 3);

                var gears = numbers.Where(x => (xRange.Contains(x.Start.X) || xRange.Contains(x.End.X)) &&
                                                (yRange.Contains(x.Start.Y) || yRange.Contains(x.End.Y)))
                                                .ToList();

                if (gears.Count >= 2)
                {
                    var result = int.Parse(gears[0].Number) * int.Parse(gears[1].Number);
                    results.Add(result);
                }

            }

            var total = results.Sum();
            Console.WriteLine(total);
        }

        static void CheckNext(char[] charList, int startingIndex)
        {
            if (startingIndex + 1 < charList.Length)
            {
                if (char.IsNumber(charList[startingIndex + 1]))
                {
                    CurrentNumber += charList[startingIndex + 1].ToString();
                    CheckNext(charList, startingIndex + 1);
                }
            }
        }
    }
}
