﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class DayTemplate
    {

        internal static int _dayNum = 9;

        public static void Start(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var inputData = Utilities.GetFileData(_dayNum, "SamplePart1.txt");
            //var inputData = Utilities.GetFileData(_dayNum, "InputData.txt");


            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

    }
}
