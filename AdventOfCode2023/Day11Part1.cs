using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class Day11Part1
    {

        internal static int _dayNum = 11;
        
        public static void Start(string[] args)
        {
            //var inputData = Utilities.GetFileData(_dayNum, "SamplePart1.txt");
            var inputData = Utilities.GetFileData(_dayNum, "InputData.txt");

            //Draw(inputData, "run1.png");

            var horizontalLines = new List<string>();

            var brokenGrid = new Dictionary<int, string>();

            for (var i = 0; i < inputData.Count; i++)
            {
                if (inputData[i].Contains("#"))
                    horizontalLines.Add(inputData[i]);
                else
                {
                    horizontalLines.Add(inputData[i]);
                    horizontalLines.Add(inputData[i]);
                }
            }

            var inputLine = inputData[0];
            var emptyLine = new List<int>();
            var columnsAdded = 0;
            for (var c = 0; c < inputLine.Length; c++)
            {
                var galaxyFound = false;
                for (var r = 0; r < inputData.Count; r++)
                {
                    inputLine = inputData[r];
                    if (inputLine.Substring(c, 1) == "#")
                    {
                        galaxyFound = true;
                        break;
                    }
                }

                if (!galaxyFound)
                {
                    for (var i = 0; i < horizontalLines.Count; i++)
                    {
                        horizontalLines[i] = horizontalLines[i].Insert(c + 1 + columnsAdded, ".");                        
                    }
                    columnsAdded++;
                }
            }

            var galaxyNum = 1;
            var grid = new List<Tile>();
            var uniqueNums = new List<string>();
            for (var r = 0; r < horizontalLines.Count; r++)
            {
                var indLine = horizontalLines[r];
                for (var c = 0; c < indLine.Length; c++)
                {
                    var value = indLine.Substring(c, 1);
                    var tileValue = value;
                    if (value != ".")
                    {
                        tileValue = galaxyNum.ToString();
                        uniqueNums.Add(tileValue);
                        galaxyNum++;
                    }
                    
                    var newTile = new Tile(c, r, tileValue);
                    grid.Add(newTile);
                }
            }

            var galaxyPairs = new List<GalaxyPair>();            
            for (var i = 0; i < uniqueNums.Count; i++)
            {
                var num1 = uniqueNums[i];
                for (var i2 = i + 1;  i2 < uniqueNums.Count; i2++)
                {
                    var num2 = uniqueNums[i2];
                    var startTile = grid.Where(x => x.Value == num1).FirstOrDefault();
                    var endTile = grid.Where(x => x.Value == num2).FirstOrDefault();

                    var newPair = new GalaxyPair(startTile, endTile);
                    galaxyPairs.Add(newPair);
                }
            }

            //Draw(horizontalLines, "run2.png");

            var sum = galaxyPairs.Sum(x => x.Distance);
            Console.WriteLine(sum);
        }

        public class GalaxyPair
        {
            public Tile First { get; set; }
            public Tile Second { get; set; }
            public int Distance { get; set; }

            public GalaxyPair(Tile first, Tile second)
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

        public class Tile
        {
            public string Value { get; set; }
            public int Column { get; set; }
            public int Row { get; set; }
            public bool IsGalaxy { get; set; }

            public Tile(int column, int row, string value)
            {
                Column = column;
                Row = row;
                Value = value;
                IsGalaxy = value != ".";
            }
        }

        public static void Draw(List<string> lines, string fileName)
        {
            var columnWidth = 60;
            var rowHeight = 60;
            var padText = 5;

            var firstLine = lines[0];
            var numRows = lines.Count;
            var numColumns = firstLine.Length;

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

                    for (var c = 0; c < numColumns; c++)
                    {
                        for (var r = 0; r < numRows; r++)
                        {
                            Brush brush = Brushes.Black;
                            var character = lines[r].Substring(c, 1);
                            g.DrawString(character, font, brush, c * columnWidth + padText, r * rowHeight + padText);
                        }
                    }
                }

                bmp.Save(@"C:\Code\AoC2023\AdventOfCode2023\Data\Day11\" + fileName, ImageFormat.Png);

            }

        }
    }
}
