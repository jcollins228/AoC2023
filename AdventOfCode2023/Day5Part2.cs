using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day5Part2
    {
        public class SeedRange
        {
            public long Start { get; set; }
            public long End { get; set; }
            public long Range { get; set; }

            public SeedRange(long start, long range)
            {
                Start = start;
                Range = range;
                End = start + (range - 1);
            }
        }

        public class MapTo
        {
            public int Order { get; set; }
            public string Type { get; set; }
            public List<Mapping> Mappings { get; set; }

            public MapTo()
            {
                Order = 0;
                Type = "";
                Mappings = new List<Mapping>();
            }

            public NewSeedData GetNewSeed(long seed)
            {
                var newSeed = new NewSeedData();

                foreach (var indMap in Mappings)
                {
                    if (indMap.InRange(seed))
                    {
                        return indMap.Convert(seed);
                    }
                }

                newSeed.NewSeed = seed;
                newSeed.RemainingLen = 0;

                return newSeed;
            }
        }

        public class Mapping
        {
            public long SourceStart { get; set; }
            public long SourceEnd { get; set; }
            public long DestinationStart { get; set; }
            public long RangeLength { get; set; }

            public Mapping(long sourceStart, long destinationStart, long rangeLength)
            {
                SourceStart = sourceStart;
                SourceEnd = sourceStart + (rangeLength - 1);
                DestinationStart = destinationStart;
                RangeLength = rangeLength;
            }

            public bool InRange(long seed)
            {
                return seed >= SourceStart && seed <= SourceEnd;
            }

            public NewSeedData Convert(long seed)
            {
                var seedData = new NewSeedData();
                var dif = seed - SourceStart;
                seedData.NewSeed = DestinationStart + dif;
                seedData.RemainingLen = RangeLength - dif;

                return seedData;
            }
        }

        public class NewSeedData
        {
            public long NewSeed { get; set; }
            public long RemainingLen { get; set; }
        }

        public static void Day5Part2_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(5, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(5, "InputData.txt");

            var seedLine = inputData[0];
            seedLine = seedLine.Replace("seeds: ", "");
            var seeds = seedLine.Split(" ").Select(s => long.Parse(s)).ToList();
            var allSeedRanges = GetSeedRanges(seeds);

            var allMappings = GetMapping(inputData);
            long? lowest = null;


            foreach (var seedRange in allSeedRanges)
            {
                for (long i = 0; i <= seedRange.Range; i++)
                {
                    var newSeed = seedRange.Start + i;
                    long? lowestDif = null;

                    foreach (var mapTo in allMappings)
                    {
                        var seedData = mapTo.GetNewSeed(newSeed);
                        if (lowestDif == null)
                            lowestDif = seedData.RemainingLen;
                        else
                            lowestDif = seedData.RemainingLen < lowestDif ? seedData.RemainingLen : lowestDif;

                        newSeed = seedData.NewSeed;
                    }
                    if (lowest == null)
                    {
                        lowest = newSeed;
                    }
                    else
                    {
                        lowest = newSeed < lowest ? newSeed : lowest;
                    }

                    if (lowestDif != null && lowestDif.Value > 0)
                        i = i + (lowestDif.Value - 1);

                }
            }

            Console.WriteLine(lowest);

        }

        public static List<SeedRange> GetSeedRanges(List<long> input)
        {
            var allRanges = new List<SeedRange>();
            for (var i = 0; i < input.Count; i = i + 2)
            {
                var start = input[i];
                var range = input[i + 1];

                var newRange = new SeedRange(start, range);
                allRanges.Add(newRange);
            }

            return allRanges;
        }

        public static List<MapTo> GetMapping(List<string> inputData)
        {
            var allMappings = new List<MapTo>();
            var mapTypes = new List<string>()
                {"seed-to-soil map:",
                "soil-to-fertilizer map:",
                "fertilizer-to-water map:",
                "water-to-light map:",
                "light-to-temperature map:",
                "temperature-to-humidity map:",
                "humidity-to-location map:" };
            var mapToOrder = 0;

            for (var i = 0; i < inputData.Count; i++)
            {
                var indLine = inputData[i];
                if (mapTypes.Contains(indLine))
                {
                    var mapTo = new MapTo();
                    mapTo.Type = indLine;
                    mapTo.Order = mapToOrder;

                    var c = i + 1;
                    var newLine = inputData[c];
                    while (newLine != "" && c < inputData.Count)
                    {
                        var data = newLine.Split(" ").Select(s => long.Parse(s)).ToList();
                        var newMap = new Mapping(data[1], data[0], data[2]);
                        mapTo.Mappings.Add(newMap);
                        c++;
                        if (c < inputData.Count)
                            newLine = inputData[c];
                        else
                            break;
                    }
                    i = c;
                    mapToOrder++;
                    allMappings.Add(mapTo);
                }
            }
            allMappings = allMappings.OrderBy(m => m.Order).ToList();
            return allMappings;
        }
    }
}
