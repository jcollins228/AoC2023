using AdventOfCode2023;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using static MyApp.Program;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        public static void Main(string[] args)
        {

            var inputData = Utilities.GetFileData(8, "SamplePart1.txt");
            //var inputData = Utilities.GetFileData(8, "InputData.txt");

            Day7Part2.Day7Part2_Main(null);
        }



    }    
}