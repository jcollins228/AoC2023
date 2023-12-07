using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day3Part1
    {
        public class PartNumber
        {
            public string? Number { get; set; }
            public int StartX { get; set; }
            public int StartY { get; set; }
        }

        public static string? CurrentNumber { get; set; }
        public static List<char>? UniqueSymbols { get; set; }

        public static void Day3Part1_Main(string[] args)
        {
            var symbolData = Utilities.GetFileData(3, "Symbols.txt");
            var uniqueList = new List<char>();
            foreach (var indLine in symbolData)
            {
                var charList = indLine.ToCharArray();
                foreach (var indChar in charList)
                {
                    if (!uniqueList.Contains(indChar)) uniqueList.Add(indChar);
                }
            }

            UniqueSymbols = uniqueList;

            //var inputData = Utilities.GetFileData(3, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(3, "InputData.txt");

            var partList = new List<int>();

            var y = 0;

            while (y < inputData.Count)
            {
                var charList = inputData[y].ToCharArray();
                var x = 0;

                while (x < charList.Length)
                {
                    if (char.IsNumber(charList[x]))
                    {
                        var newPart = new PartNumber() { StartX = x, StartY = y };
                        CurrentNumber = charList[x].ToString();
                        CheckNext(charList, x);
                        newPart.Number = CurrentNumber;
                        x = x + CurrentNumber.Length;

                        var hasSymbol = NumberHasSymbol(inputData, newPart.StartX, newPart.StartY, newPart.Number);
                        if (hasSymbol)
                        {
                            partList.Add(int.Parse(newPart.Number));
                        }
                    }
                    else
                    {
                        x++;
                    }
                }
                y++;
            }

            var sum = partList.Sum();
            Console.WriteLine(sum.ToString());
        }

        static bool NumberHasSymbol(List<string> inputData, int startX, int startY, string number)
        {
            var hasSymbol = false;

            var startXCheck = startX - 1;
            var endXCheck = startX + number.Length + 1;
            var startYCheck = startY - 1;
            var endYCheck = startY + 2;

            var currentX = startXCheck;
            var currentY = startYCheck;

            while (currentY < endYCheck)
            {
                if (currentY >= 0 && currentY < inputData.Count)
                {
                    var currentLine = inputData[currentY];
                    currentX = startXCheck;
                    while (currentX < endXCheck)
                    {
                        if (currentX >= 0 && currentX < currentLine.Length)
                        {
                            var currentChar = char.Parse(currentLine.Substring(currentX, 1));
                            if (UniqueSymbols != null && UniqueSymbols.Contains(currentChar))
                            {
                                return true;
                            }
                            else
                            {
                                currentX++;
                            }
                        }
                        else
                        {
                            currentX++;
                        }
                    }
                    currentY++;
                }
                else
                {
                    currentY++;
                }
            }

            return hasSymbol;
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
