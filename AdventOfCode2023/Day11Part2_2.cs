using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2023.Day11Part2_2;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day11Part2_2
    {

        internal static int _dayNum = 11;
        
        public static void Start(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //var inputData = Utilities.GetFileData(_dayNum, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(_dayNum, "InputData.txt");

            //Draw(inputData, "run1.png");
            
            var galaxyExpansion = 999999;
            var allGalaxies = new List<Galaxy>();
            var galaxyNum = 1;


            // find all galaxies and re-number them
            for (var r = 0; r < inputData.Count; r++)
            {
                var indLine = inputData[r];
                var galaxyIndexs = Utilities.IndexOfAny(indLine, '#');

                for (var i = 0; i < galaxyIndexs.Count; i++)
                {
                    var galaxyColumn = galaxyIndexs[i];
                    var indGalaxy = new Galaxy(galaxyColumn, r, galaxyNum.ToString());
                    allGalaxies.Add(indGalaxy);
                    galaxyNum++;
                }
            }

            // find blank rows and create column strings
            var allColumns = new Dictionary<int, string>();
            var blankRows = new List<int>();
            
            for (var r = 0; r < inputData.Count; r++)
            {                
                var indLine = inputData[r];
                if (!indLine.Contains("#"))
                    blankRows.Add(r);

                var lineChar = indLine.ToCharArray();
                for (var c = 0; c < lineChar.Length; c++)                
                {
                    var indChar = lineChar[c];
                    if (allColumns.ContainsKey(c))
                    {
                        allColumns[c] = allColumns[c] + indChar;
                    }
                    else
                    {
                        allColumns.Add(c, indChar.ToString());
                    }
                }
            }

            // find blank columns
            var blankColumns = new List<int>();
            for (var c = 0; c < allColumns.Keys.Count; c++)
            {
                var indColumn = allColumns[c];
                if (!indColumn.Contains("#"))
                    blankColumns.Add(c);
            }

            // expand row values
            var rowsAdd = 0;
            foreach (var r in blankRows)
            {
                var galaxiesToMove = allGalaxies.Where(x => x.Row > r + rowsAdd).ToList();
                foreach (var indGalaxy in galaxiesToMove)
                {
                    indGalaxy.Row += galaxyExpansion;
                }
                rowsAdd += galaxyExpansion;
            }

            // expand column values
            var columnsAdd = 0;
            foreach (var c in blankColumns)
            {
                var galaxiesToMove = allGalaxies.Where(x => x.Column > c + columnsAdd).ToList();
                foreach (var indGalaxy in galaxiesToMove)
                {
                    indGalaxy.Column += galaxyExpansion;
                }
                columnsAdd += galaxyExpansion;
            }

            // find/create pairs which calculates distance
            var galaxyPairs = new List<GalaxyPair>();
            for (var i = 0; i < allGalaxies.Count; i++)
            {
                var galaxy1 = allGalaxies[i];
                for (var i2 = i + 1; i2 < allGalaxies.Count; i2++)
                {
                    var galaxy2 = allGalaxies[i2];
                    var newPair = new GalaxyPair(galaxy1, galaxy2);
                    galaxyPairs.Add(newPair);
                }
            }

            long sum = galaxyPairs.Sum(x => x.Distance);
            Console.WriteLine(sum.ToString());

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            //Draw(allGalaxies, "Part2.png");

        }

        public class GalaxyPair
        {
            public Galaxy First { get; set; }
            public Galaxy Second { get; set; }
            public long Distance { get; set; }

            public GalaxyPair(Galaxy first, Galaxy second)
            {
                First = first;
                Second = second;

                var horizontalSteps = 0;
                var vertiacalSteps = 0;

                if (first.Column == second.Column)
                    horizontalSteps = 0;
                else if (first.Column < second.Column)
                    horizontalSteps = second.Column - first.Column;
                else
                    horizontalSteps = first.Column - second.Column;

                if (first.Row == second.Row)
                    vertiacalSteps = 0;
                else if (first.Row < second.Row)
                    vertiacalSteps = second.Row - first.Row;
                else
                    vertiacalSteps = first.Row - second.Row;

                Distance = horizontalSteps + vertiacalSteps;
            }
        }

        public class Galaxy
        {
            public string Value { get; set; }
            public int Column { get; set; }
            public int Row { get; set; }            

            public Galaxy(int column, int row, string value)
            {
                Column = column;
                Row = row;
                Value = value;                
            }
        }

        public static void Draw(List<Galaxy> allGalaxies, string fileName)
        {
            var columnWidth = 60;
            var rowHeight = 60;
            var padText = 5;

            var numRows = allGalaxies.Max(x => x.Row) + 1;
            var numColumns = allGalaxies.Max(x => x.Column) + 1;

            using (var bmp = new Bitmap(numColumns * columnWidth, numRows * rowHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    var blackPen = new Pen(Color.Black);
                    var redPen = new Pen(Color.Red);

                    for (var x = 0; x < numColumns; x++)
                    {
                        var x1 = x * columnWidth;
                        var y1 = 0;

                        var x2 = x * columnWidth;
                        var y2 = numRows * rowHeight;

                        g.DrawLine(blackPen, x1, y1, x2, y2);
                    }

                    for (var y = 0; y < numRows; y++)
                    {
                        var x1 = 0;
                        var y1 = y * rowHeight;

                        var x2 = numColumns * columnWidth;
                        var y2 = y * rowHeight;

                        g.DrawLine(blackPen, x1, y1, x2, y2);
                    }

                    var font = new Font(FontFamily.GenericMonospace, 14);

                    foreach (var indGalaxy in allGalaxies)
                    {
                        Brush brush = Brushes.Black;
                        g.DrawString(indGalaxy.Value, font, brush, indGalaxy.Column * columnWidth + padText, indGalaxy.Row * rowHeight + padText);
                    }

                }

                bmp.Save(@"C:\Code\AoC2023\AdventOfCode2023\Data\Day11\" + fileName, ImageFormat.Png);

            }

        }

    }
}
