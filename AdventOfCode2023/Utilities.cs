using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Utilities
    {

        const string filePath = @"C:\Code\AoC2023\AdventOfCode2023\Data\Day";

        public static List<string> GetFileData(int day, string file)
        {
            var completePath = $"{filePath}{day.ToString()}\\{file}";
            var inpputFile = File.ReadAllLines(completePath);
            var inputList = new List<string>(inpputFile);

            return inputList;
        }

        public static List<int> IndexOfAny(string original, char searchValue)
        {
            var foundIndexes = new List<int>();
            for (int i = original.IndexOf(searchValue); i > -1; i = original.IndexOf(searchValue, i + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
            }
            return foundIndexes;
        }
    }
}
