using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day2
    {
        private class GameInfo
        {
            public int GameNumber { get; set; }
            public List<CubeInfo> CubeSets { get; set; }

            public GameInfo()
            {
                CubeSets = new List<CubeInfo>();
            }
        }

        private class CubeInfo
        {
            public int Blue { get; set; }
            public int Red { get; set; }
            public int Green { get; set; }

            public CubeInfo()
            {
                Blue = 0;
                Red = 0;
                Green = 0;
            }
        }

        public static void Day2Part2_Main(string[] args)
        {
            var inpputFile = File.ReadAllLines(@"C:\Code\AdventOfCode2023\Day 2\InputPart1.txt");
            var inputList = new List<string>(inpputFile);

            var allGames = new List<GameInfo>();

            foreach (var indGame in inputList)
            {
                var gameInfo = new GameInfo();
                gameInfo.GameNumber = int.Parse(indGame.Substring(0, indGame.IndexOf(':')).Replace("Game ", ""));

                var startIndex = indGame.IndexOf(":") + 2;
                var setData = indGame.Substring(startIndex, indGame.Length - startIndex).Split(';');
                foreach (var indSet in setData)
                {
                    var cubeSplit = indSet.Split(',');
                    var cubeInfo = new CubeInfo();
                    foreach (var indCube in cubeSplit)
                    {
                        var cubeData = indCube.Trim().Split(" ");
                        switch (cubeData[1])
                        {
                            case "red":
                                cubeInfo.Red = int.Parse(cubeData[0]);
                                break;
                            case "blue":
                                cubeInfo.Blue = int.Parse(cubeData[0]);
                                break;
                            case "green":
                                cubeInfo.Green = int.Parse(cubeData[0]);
                                break;
                        }

                    }
                    gameInfo.CubeSets.Add(cubeInfo);
                }
                allGames.Add(gameInfo);
            }

            var powersList = new List<int>();
            foreach (var indGame in allGames)
            {
                var maxRed = indGame.CubeSets.Max(x => x.Red);
                var maxGreen = indGame.CubeSets.Max(x => x.Green);
                var maxBlue = indGame.CubeSets.Max(y => y.Blue);

                var gameSetPower = maxRed * maxGreen * maxBlue;
                powersList.Add(gameSetPower);
            }

            var total = powersList.Sum();
            Console.WriteLine(total.ToString());
        }

        public static void Day2Part1_Main(string[] args)
        {
            var inpputFile = File.ReadAllLines(@"C:\Code\AdventOfCode2023\Day 2\InputPart1.txt");
            var inputList = new List<string>(inpputFile);

            var allGames = new List<GameInfo>();

            foreach (var indGame in inputList)
            {
                var gameInfo = new GameInfo();
                gameInfo.GameNumber = int.Parse(indGame.Substring(0, indGame.IndexOf(':')).Replace("Game ", ""));

                var startIndex = indGame.IndexOf(":") + 2;
                var setData = indGame.Substring(startIndex, indGame.Length - startIndex).Split(';');
                foreach (var indSet in setData)
                {
                    var cubeSplit = indSet.Split(',');
                    var cubeInfo = new CubeInfo();
                    foreach (var indCube in cubeSplit)
                    {
                        var cubeData = indCube.Trim().Split(" ");
                        switch (cubeData[1])
                        {
                            case "red":
                                cubeInfo.Red = int.Parse(cubeData[0]);
                                break;
                            case "blue":
                                cubeInfo.Blue = int.Parse(cubeData[0]);
                                break;
                            case "green":
                                cubeInfo.Green = int.Parse(cubeData[0]);
                                break;
                        }

                    }
                    gameInfo.CubeSets.Add(cubeInfo);
                }
                allGames.Add(gameInfo);
            }

            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;

            var validGameNumers = new List<int>();
            foreach (var indGame in allGames)
            {
                var allGood = true;

                foreach (var indSet in indGame.CubeSets)
                {
                    if ((indSet.Red > maxRed || indSet.Green > maxGreen || indSet.Blue > maxBlue) && allGood)
                    {
                        allGood = false;
                    }
                }

                if (allGood)
                {
                    validGameNumers.Add(indGame.GameNumber);
                }
            }

            var total = validGameNumers.Sum();
            Console.WriteLine(total.ToString());
        }
    }
}
