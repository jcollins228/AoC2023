using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    public static class Day10Part1
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

        public class Tile
        {
            public char Value { get; set; }
            public Direction? DirectionFrom { get; set; }
            public Direction? DirectionTo { get; set; }
            public Location Coordinates { get; set; }
            public bool IsStart { get; set; }
            public List<Direction> ValidDirections { get; set; }

            public Tile(char value, int x, int y)
            {
                Value = value;
                Coordinates = new Location(x, y);
                DirectionFrom = null;
                DirectionTo = null;
                IsStart = false;                

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
                        ValidDirections = new List<Direction>();
                        IsStart = true;
                        break;
                    case '.':
                        ValidDirections = new List<Direction>();
                        
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

                return newTile;
            }
        }

        public static void Day10Part1_Main(string[] args)
        {
            //var inputData = Utilities.GetFileData(_dayNum, "SamplePart1_2.txt");
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
                var path = new List<Tile>() { startingTile.Clone(), northTile };
                paths.Add(path);
            }

            // check east                
            var eastTile = GetTileFromDirection(grid, startingTile, Direction.east);
            if (eastTile != null)
            {
                var path = new List<Tile>() { startingTile.Clone(), eastTile };
                paths.Add(path);
            }

            // check west                
            var westTile = GetTileFromDirection(grid, startingTile, Direction.west);
            if (westTile != null)
            {
                var path = new List<Tile>() { startingTile.Clone(), westTile };
                paths.Add(path);
            }

            // check south                
            var southTile = GetTileFromDirection(grid, startingTile, Direction.south);
            if (southTile != null)
            {
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
                            indPath.Add(nextTile);
                        }
                    }
                } while (nextTile != null);
            }

            var result = paths[0].Count / 2;
            Console.Write(result);
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
                    var directionTo  = newTile.ValidDirections.Where(x => x != directionFrom).FirstOrDefault();
                    newTile.DirectionTo = directionTo;
                }                
            }

            return newTile;
        }



    }
}
