using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    public static class Day9Part1
    {

        internal static int _dayNum = 9;

        public static void Day9Part1_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(_dayNum, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(_dayNum, "InputData.txt");

            var sumValues = new List<long>();
            foreach (var indLine in inputData)
            {
                var c = 1;                
                var data = indLine.Split(' ').Select(x => int.Parse(x)).ToList();
                var differenceList = new Dictionary<int, List<int>>();
                differenceList.Add(0, data);
                GetDifference(data, differenceList, c);

                var difSort = differenceList.Keys.OrderByDescending(x => x).ToList();
                var addValue = 0;
                for (var i = difSort.Max(); i >=0; i--)
                {
                    var indSet = differenceList[i];
                    var newVal = indSet[indSet.Count - 1] + addValue;
                    indSet.Add(newVal);
                    addValue = newVal;
                }

                var firstList = differenceList[0];
                sumValues.Add(firstList[firstList.Count - 1]);
            }

            var result = sumValues.Sum();
            Console.WriteLine(result);
        }

        public static void GetDifference(List<int> data, Dictionary<int, List<int>> differenceList, int diffCount)
        {
            var difference = new List<int>();
            var end = true;
            for (var i = 0; i < data.Count - 1; i++)
            {
                var dif = data[i +1] - data[i];
                difference.Add(dif);
                if (dif != 0)
                    end = false;
            }

            differenceList.Add(diffCount, difference);
            if (!end)
                GetDifference(difference, differenceList, diffCount + 1);            
        }

        
    }
}
