using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day5Part1
    {
        public class MapTo
        {
            public int Order { get; set; }
            public string Type { get; set; }
            public List<Mapping> Mappings { get; set; }

            public MapTo()
            {
                Mappings = new List<Mapping>();
                Order = 0;
                Type = "";
            }

            public long GetNewSeed(long seed)
            {
                foreach (var indMap in Mappings)
                {
                    if (indMap.InRange(seed))
                    {
                        return indMap.Convert(seed);
                    }
                }

                return seed;
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

            public long Convert(long seed)
            {
                var dif = seed - SourceStart;
                return DestinationStart + dif;
            }
        }

        public static void Day5Part1_Main(string[] args)
        {
            var inputData = Utilities.GetFileData(5, "SamplePart1.txt");
            //var inputData = Utilities.GetFileData(5, "InputData.txt");

            var seedLine = inputData[0];
            seedLine = seedLine.Replace("seeds: ", "");
            var seeds = seedLine.Split(" ").Select(s => long.Parse(s)).ToList();

            var allMappings = GetMapping(inputData);
            var allFinals = new List<long>();

            foreach (var seed in seeds)
            {
                var newSeed = seed;
                foreach (var mapTo in allMappings)
                {
                    newSeed = mapTo.GetNewSeed(newSeed);
                }
                allFinals.Add(newSeed);
            }

            var result = allFinals.Min();
            Console.WriteLine(result);

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
