using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day6
    {
        public class Race
        {
            public int RaceNum { get; set; }
            public int Time { get; set; }
            public int Distance { get; set; }
            public int PossibleWins { get; set; }

            public Race()
            {
                PossibleWins = 0;
            }
        }

        public static void Part2(string[] args)
        {
            //var inputData = Utilities.GetFileData(6, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(6, "InputData.txt");

            var timeData = long.Parse(inputData[0].Replace("Time:", "").Replace(" ", ""));
            var distanceData = long.Parse(inputData[1].Replace("Distance:", "").Replace(" ", ""));

            var possibleWins = 0;
            for (var i = 1; i < timeData; i++)
            {
                var timeHeld = i;
                var remainingTime = timeData - timeHeld;
                var distance = timeHeld * remainingTime;
                if (distance > distanceData)
                {
                    possibleWins++;
                }
            }

            Console.Write(possibleWins);
        }

        public static void Part1(string[] args)
        {
            //var inputData = Utilities.GetFileData(6, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(6, "InputData.txt");

            var timeData = inputData[0].Replace("Time:", "").Trim().Split(' ').ToList().Where(x => x != "").Select(x => int.Parse(x)).ToList();
            var distanceData = inputData[1].Replace("Distance:", "").Trim().Split(' ').ToList().Where(x => x != "").Select(x => int.Parse(x)).ToList();

            var raceData = new List<Race>();
            for (var i = 0; i < timeData.Count; i++)
            {
                var race = new Race() { RaceNum = i + 1, Time = timeData[i], Distance = distanceData[i] };
                raceData.Add(race);
            }

            foreach (var indRace in raceData)
            {
                for (var i = 1; i < indRace.Time; i++)
                {
                    var timeHeld = i;
                    var remainingTime = indRace.Time - timeHeld;
                    var distance = timeHeld * remainingTime;
                    if (distance > indRace.Distance)
                    {
                        indRace.PossibleWins++;
                    }
                }
            }

            var answer = raceData[0].PossibleWins;
            for (var i = 1; i < raceData.Count; i++)
            {
                answer = answer * raceData[i].PossibleWins;
            }

            Console.Write(answer);
        }
    }
}
