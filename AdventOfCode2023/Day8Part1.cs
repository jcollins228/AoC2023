using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day8Part1
    {
        public class Entry
        {
            public string Left { get; set; } = "";
            public string Right { get; set; } = "";

            public string GetValue(char instruction)
            {
                if (instruction == 'L')
                    return Left;
                else
                    return Right;
            }
        }

        public static void Day8Part1_Main(string[] args)
        {

            //var inputData = Utilities.GetFileData(8, "SamplePart1_2.txt");
            var inputData = Utilities.GetFileData(8, "InputData.txt");

            var instructions = inputData[0].ToCharArray();
            var completeList = new Dictionary<string, string[]>();

            string? firstResult = null;

            for (var i = 2; i < inputData.Count; i++)
            {
                var lineData = inputData[i];
                var start = lineData.Substring(0, 3);
                var splitDirc = lineData.Substring(7, lineData.Length - 7).Replace(")", "").Replace(" ", "").Split(',');

                completeList.Add(start, splitDirc);

                if (firstResult == null)
                    firstResult = start;
            }

            var found = false;
            var z = 0;
            long steps = 0;
            string lastResult = "AAA";

            while (!found)
            {
                if (z >= instructions.Length)
                    z = 0;

                var currentIns = instructions[z];
                var currentSet = completeList[lastResult];
                lastResult = currentIns == 'L' ? currentSet[0] : currentSet[1];

                steps++;
                z++;
                if (lastResult == "ZZZ")
                    found = true;
            }

            Console.Write(steps.ToString());
        }
    }
}
