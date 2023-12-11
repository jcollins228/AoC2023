using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdventOfCode2023
{
    public static class Day10Part2
    {

        internal static int _dayNum = 10;

        public enum Direction
        {
            north,
            south,
            east,
            west
        }

        public class Location
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Location(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public class Tile : IEquatable<Tile>
        {
            public char Value { get; set; }
            public Direction? DirectionFrom { get; set; }
            public Direction? DirectionTo { get; set; }
            public Location Coordinates { get; set; }
            public bool IsStart { get; set; }
            public List<Direction> ValidDirections { get; set; }
            public int StepNum { get; set; }

            public Tile(char value, int x, int y)
            {
                Value = value;
                Coordinates = new Location(x, y);
                DirectionFrom = null;
                DirectionTo = null;
                IsStart = false;
                StepNum = 0;
                ValidDirections = new List<Direction>();

                switch (value)
                {
                    case '|':
                        ValidDirections = new List<Direction>() { Direction.south, Direction.north };
                        break;
                    case '-':
                        ValidDirections = new List<Direction>() { Direction.east, Direction.west };
                        break;
                    case 'L':
                        ValidDirections = new List<Direction>() { Direction.north, Direction.east };
                        break;
                    case 'J':
                        ValidDirections = new List<Direction>() { Direction.north, Direction.west };
                        break;
                    case '7':
                        ValidDirections = new List<Direction>() { Direction.south, Direction.west };
                        break;
                    case 'F':
                        ValidDirections = new List<Direction>() { Direction.south, Direction.east };
                        break;
                    case 'S':                        
                        IsStart = true;
                        break;
                    case '.':
                        break;
                }
            }

            public Tile Clone() 
            {
                var newTile = new Tile(Value, Coordinates.X, Coordinates.Y);
                newTile.DirectionFrom = DirectionFrom;
                newTile.DirectionTo = DirectionTo;
                newTile.IsStart = IsStart;
                newTile.ValidDirections = ValidDirections.Select(x => x).ToList();
                newTile.StepNum = StepNum;

                return newTile;
            }

            public static bool operator ==(Tile? t1, Tile? t2)
            {
                if (ReferenceEquals(t1, t2))
                    return true;
                if (ReferenceEquals(t1, null))
                    return false;
                if (ReferenceEquals(t2, null))
                    return false;

                return t1.Value == t2.Value
                    && t1.Coordinates.X == t2.Coordinates.X
                    && t1.Coordinates.Y == t2.Coordinates.Y;
            }

            public static bool operator !=(Tile? t1, Tile? t2)
            {
                return !(t1 == t2);
            }

            public override bool Equals(object? other)
            {
                return this.Equals(other as Tile);
            }

            public bool Equals(Tile? other)
            {
                if (other == null)
                    return false;                               

                return this.Value == other.Value
                    && this.Coordinates.X == other.Coordinates.X
                    && this.Coordinates.Y == other.Coordinates.Y;
            }
        }

        public static void Day10Part2_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(_dayNum, "SamplePart2_1.txt");
            var inputData = Utilities.GetFileData(_dayNum, "InputData.txt");

            var grid = new List<Tile>();

            for (var y = 0; y < inputData.Count; y++)
            {
                var lineData = inputData[y].ToCharArray();
                for (var x = 0; x < lineData.Length; x++)
                {
                    var tile = new Tile(lineData[x], x, y);
                    grid.Add(tile);
                }
            }

            var startingTile = grid.Where(x => x.IsStart).FirstOrDefault();
            var paths = new List<List<Tile>>();


            // check north                
            var northTile = GetTileFromDirection(grid, startingTile, Direction.north);
            if (northTile != null)
            {
                northTile.StepNum++;
                var path = new List<Tile>() { startingTile.Clone(), northTile };
                paths.Add(path);
            }

            // check east                
            var eastTile = GetTileFromDirection(grid, startingTile, Direction.east);
            if (eastTile != null)
            {
                eastTile.StepNum++;
                var path = new List<Tile>() { startingTile.Clone(), eastTile };
                paths.Add(path);
            }

            // check west                
            var westTile = GetTileFromDirection(grid, startingTile, Direction.west);
            if (westTile != null)
            {
                westTile.StepNum++;
                var path = new List<Tile>() { startingTile.Clone(), westTile };
                paths.Add(path);
            }

            // check south                
            var southTile = GetTileFromDirection(grid, startingTile, Direction.south);
            if (southTile != null)
            {
                southTile.StepNum++;
                var path = new List<Tile>() { startingTile.Clone(), southTile };
                paths.Add(path);
            }


            foreach (var indPath in paths)
            {
                Tile? nextTile = null;

                do
                {
                    var lastTile = indPath.LastOrDefault();
                    if (lastTile != null)
                    {
                        nextTile = GetTileFromDirection(grid, lastTile, lastTile.DirectionTo.Value);
                        if (nextTile != null)
                        {                          
                            nextTile.StepNum = lastTile.StepNum + 1;
                            indPath.Add(nextTile);
                        }
                    }
                } while (nextTile != null);
            }

            var maxX = grid.Max(t => t.Coordinates.X);
            var maxY = grid.Max(t => t.Coordinates.Y);
            var path1 = paths[0];
            var path2 = paths[1];            
                        
            var getS = path1.Where(x => x.Value == 'S').First();
            getS.DirectionFrom = GetOppositeDirection(path1[1].DirectionFrom.Value);
            getS.DirectionTo = GetOppositeDirection(path2[1].DirectionFrom.Value);

            var notInPath = grid.Where(t => !path1.Contains(t)).ToList();

            var insideTiles = new List<Tile>();
            foreach (var checkTile in notInPath)
            {                
                var pathTiles = path1.Where(t => t.Coordinates.Y == checkTile.Coordinates.Y && t.Coordinates.X > checkTile.Coordinates.X).ToList();
                var i = 0;
                Direction? startDirection = null;
                Direction? endDirection = null;
                var numCrosses = 0;
                                
                while (i < pathTiles.Count)
                {
                    var currentTile = pathTiles[i];
                    if (currentTile.Value == '|')
                    {
                        numCrosses++;
                        i++;
                    } 
                    else if (startDirection == null && (currentTile.DirectionFrom == Direction.north || currentTile.DirectionFrom == Direction.south))
                    {
                        if (startDirection == null)
                            startDirection = currentTile.DirectionFrom;
                        i++;
                    }
                    else if (endDirection == null  && (currentTile.DirectionTo == Direction.north || currentTile.DirectionTo == Direction.south))
                    {
                        if (endDirection == null)
                            endDirection = currentTile.DirectionTo;

                        if (startDirection == endDirection)
                        {
                            i++;
                            startDirection = null;
                            endDirection = null;
                        }
                        else
                        {
                            numCrosses++;

                            i++;
                            startDirection = null;
                            endDirection = null;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }

                if (pathTiles.Count > 0)
                {
                    var isEven = numCrosses % 2 == 0;
                    if (!isEven)
                    {
                        insideTiles.Add(checkTile);
                        checkTile.StepNum = -555;
                    }
                }
                
            }


            //var newPath = paths[0];
            //var numColumns = grid.Max(x => x.Coordinates.Y) + 1;
            //var numRows = grid.Max(y => y.Coordinates.X) + 1;

            //Draw(grid, paths, numColumns, numRows);

            var result = paths[0].Count / 2;
            Console.WriteLine(result);

            var changed = grid.Where(x => x.StepNum == -555).ToList();
            Console.WriteLine(changed.Count());

            Console.WriteLine(insideTiles.Count());
        }

        public static Direction GetOppositeDirection(Direction current)
        {
            switch (current)
            {
                case Direction.north:
                    return Direction.south;
                case Direction.south:
                    return Direction.north;
                case Direction.east:
                    return Direction.west;
                case Direction.west:
                    return Direction.east;
            }

            return Direction.north;
        }

        public static Tile? GetTileFromDirection(List<Tile> grid, Tile currentTile, Direction direction)
        {
            Tile? newTile = null;
            var x = 0;
            var y = 0;
            Direction? directionFrom = null;

            switch (direction)
            {
                case Direction.north:
                    x = currentTile.Coordinates.X;
                    y = currentTile.Coordinates.Y - 1;
                    directionFrom = Direction.south;
                    break;
                case Direction.south:
                    x = currentTile.Coordinates.X;
                    y = currentTile.Coordinates.Y + 1;
                    directionFrom = Direction.north;
                    break;
                case Direction.east:
                    x = currentTile.Coordinates.X + 1;
                    y = currentTile.Coordinates.Y;
                    directionFrom = Direction.west;
                    break;
                case Direction.west:
                    x = currentTile.Coordinates.X - 1;
                    y = currentTile.Coordinates.Y;
                    directionFrom = Direction.east;
                    break;
            }

            if (x >= 0 && y >= 0)
            {
                var tile = grid.Where(t => t.Coordinates.X == x && t.Coordinates.Y == y && t.ValidDirections.Contains(directionFrom.Value)).FirstOrDefault();
                if (tile != null)
                {
                    newTile = tile.Clone();                    
                    newTile.DirectionFrom = directionFrom;
                    var directionTo = newTile.ValidDirections.Where(x => x != directionFrom).FirstOrDefault();
                    newTile.DirectionTo = directionTo;
                }
            }

            return newTile;
        }

        public static void Draw(List<Tile> grid, List<List<Tile>> paths, int numRows, int numColumns)
        {
            var columnWidth = 60;
            var rowHeight = 60;
            var padText = 5;

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

                    var newPaths = new List<List<Tile>>();

                    var path1 = paths[0].GetRange(0, paths[0].Count / 2 + 1);
                    var path2 = paths[1].GetRange(0, paths[1].Count / 2 + 1);
                                        
                    foreach (var indTile in grid)
                    {
                        Brush brush = Brushes.Black;
                        Tile tileToDraw = indTile;

                        if (indTile.Value == 'S')
                            brush = Brushes.YellowGreen;
                        else if (indTile.Coordinates.X == path1.Last().Coordinates.X && indTile.Coordinates.Y == path1.Last().Coordinates.Y)
                        {
                            brush = Brushes.YellowGreen;
                            tileToDraw = path1.Last();
                        }
                        else if (indTile.Value == '.')
                            brush = Brushes.Black;
                        else
                        {
                            var inPath1 = path1.Where(t => t.Coordinates.X == indTile.Coordinates.X && t.Coordinates.Y == indTile.Coordinates.Y).ToList();
                            var inPath2 = path2.Where(t => t.Coordinates.X == indTile.Coordinates.X && t.Coordinates.Y == indTile.Coordinates.Y).ToList();
                            if (inPath1.Count > 0)
                            {
                                brush = Brushes.Red;
                                tileToDraw = inPath1[0];
                            }
                                
                            if (inPath2.Count > 0)
                            {
                                brush = Brushes.Blue;
                                tileToDraw = inPath2[0];
                            }
                                
                        }
                        
                        var stepNum = tileToDraw.StepNum == 0 && tileToDraw.Value != 'S' ? "" : tileToDraw.StepNum.ToString();

                        g.DrawString(tileToDraw.Value.ToString(), font, brush, indTile.Coordinates.X * columnWidth + padText, indTile.Coordinates.Y * rowHeight + padText);
                        g.DrawString(stepNum, font, brush, indTile.Coordinates.X * columnWidth + padText, indTile.Coordinates.Y * rowHeight + padText + 30);
                    }
                }

                bmp.Save(@"C:\Code\AoC2023\AdventOfCode2023\Data\Day10\testing.png", ImageFormat.Png);

            }
        }

    }
}
