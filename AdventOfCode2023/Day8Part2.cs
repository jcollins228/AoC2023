using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day8Part2
    {
        internal static int _dayNum = 8;

        public static void Day8Part2_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(8, "SamplePart2.txt");
            var inputData = Utilities.GetFileData(8, "InputData.txt");

            var instructions = inputData[0].ToCharArray();
            var completeList = new Dictionary<string, string[]>();

            for (var i = 2; i < inputData.Count; i++)
            {
                var lineData = inputData[i];
                var start = lineData.Substring(0, 3);
                var splitDirc = lineData.Substring(7, lineData.Length - 7).Replace(")", "").Replace(" ", "").Split(',');

                completeList.Add(start, splitDirc);
            }

            var startingList = completeList.Keys.Where(x => x.EndsWith("A")).ToArray();
            var stepList = new List<long>();

            for (var i = 0; i < startingList.Length; i++)
            {
                var z = 0;
                long steps = 0;
                var found = false;

                var startVal = startingList[i];


                while (!found)
                {
                    if (z >= instructions.Length)
                        z = 0;

                    var currentIns = instructions[z];
                    startVal = currentIns == 'L' ? completeList[startVal][0] : completeList[startVal][1];

                    steps++;
                    z++;

                    if (startVal.Substring(2, 1) == "Z")
                    {
                        found = true;
                        stepList.Add(steps);
                    }
                }
            }
        }

    }
}
