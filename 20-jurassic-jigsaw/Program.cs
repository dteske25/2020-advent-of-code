using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JurassicJigsaw
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\JurassicJigsaw\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();
            var tiles = PartOne(lines);
            PartTwo(tiles);
        }

        private static void PartTwo(List<Tile> tiles)
        {
            var tileArangement = new List<List<Tile>>();
            tiles = tiles.OrderBy(t => t.MatchingSides.Count).ToList();

            tiles[0].X = 0;
            tiles[0].Y = 0;

            var adjustedTiles = new HashSet<int>();
            var tilesToAdjust = new List<Tile>();
            tilesToAdjust.Add(tiles[0]);
            adjustedTiles.Add(tiles[0].Id);
            while (tilesToAdjust.Count > 0)
            {
                var currentTile = tilesToAdjust[0];
                tilesToAdjust.Remove(currentTile);
                foreach (var otherTile in currentTile.MatchingSides.Where(c => !adjustedTiles.Contains(c.Id)).ToList())
                {
                    MatchAndAlign(currentTile, otherTile);
                    if (!adjustedTiles.Contains(otherTile.Id))
                    {
                        tilesToAdjust.Add(otherTile);
                        adjustedTiles.Add(otherTile.Id);
                    }
                }
            }

            var maxX = tiles.Max(t => t.X);
            var minX = tiles.Min(t => t.X);
            var maxY = tiles.Max(t => t.Y);
            var minY = tiles.Min(t => t.Y);

            for (int x = minX; x <= maxX; x++)
            {
                tileArangement.Add(new List<Tile>());
                for (int y = minY; y <= maxY; y++)
                {
                    var foundTile = tiles.First(t => t.X == x && t.Y == y);
                    tileArangement[x].Add(foundTile);
                }
            }

            tileArangement.Reverse();
            var solvedPuzzle = new List<List<char>>();
            foreach (var row in tileArangement)
            {
                solvedPuzzle.AddRange(CombinePuzzleRow(row));
                Console.WriteLine();
            }


            var regexLn1 = "..................#.";
            var regexLn2 = "#....##....##....###";
            var regexLn3 = ".#..#..#..#..#..#...";

            

            // 145 is too low
            Console.WriteLine($"Solution 2: ");
        }


        private static List<List<char>> CombinePuzzleRow(List<Tile> row)
        {
            var tileHeight = row[0].TrimmedData.Count;
            var result = new List<List<char>>();
            for (int i = 0; i < tileHeight; i++)
            {
                result.Add(new List<char>());
                foreach (var tile in row)
                {
                    result[i].AddRange(tile.TrimmedData[i]);
                    Console.Write($"{string.Join("", tile.TrimmedData[i])} ");
                }
                Console.WriteLine();
            }
            return result;
        }

        private static void MatchAndAlign(Tile baseTile, Tile otherTile)
        {
            var sharedBorder = baseTile.Borders.First(b => otherTile.Borders.Contains(b));
            var x = baseTile.Borders.IndexOf(sharedBorder);
            var y = otherTile.Borders.IndexOf(sharedBorder);

            if (y == 4 || y == 5)
            {
                otherTile.FlipDataHorizontally();
                y = otherTile.Borders.IndexOf(sharedBorder);
            }
            else if (y == 6 || y == 7)
            {
                otherTile.FlipDataVertically();
                y = otherTile.Borders.IndexOf(sharedBorder);
            }

            if (x == 0)
            {
                otherTile.Y = baseTile.Y;
                otherTile.X = baseTile.X + 1;
                while (y != 1)
                {
                    otherTile.RotateData();
                    y = otherTile.Borders.IndexOf(sharedBorder);
                    if (y == 4 || y == 5)
                    {
                        otherTile.FlipDataHorizontally();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                    else if (y == 6 || y == 7)
                    {
                        otherTile.FlipDataVertically();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                }
            }
            else if (x == 1)
            {
                otherTile.Y = baseTile.Y;
                otherTile.X = baseTile.X - 1;
                while (y != 0)
                {
                    otherTile.RotateData();
                    y = otherTile.Borders.IndexOf(sharedBorder);
                    if (y == 4 || y == 5)
                    {
                        otherTile.FlipDataHorizontally();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                    else if (y == 6 || y == 7)
                    {
                        otherTile.FlipDataVertically();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                }
            }
            else if (x == 2)
            {
                otherTile.Y = baseTile.Y - 1;
                otherTile.X = baseTile.X;
                while (y != 3)
                {
                    otherTile.RotateData();
                    y = otherTile.Borders.IndexOf(sharedBorder);
                    if (y == 4 || y == 5)
                    {
                        otherTile.FlipDataHorizontally();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                    else if (y == 6 || y == 7)
                    {
                        otherTile.FlipDataVertically();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                }
            }
            else if (x == 3)
            {
                otherTile.Y = baseTile.Y + 1;
                otherTile.X = baseTile.X;
                while (y != 2)
                {
                    otherTile.RotateData();
                    y = otherTile.Borders.IndexOf(sharedBorder);
                    if (y == 4 || y == 5)
                    {
                        otherTile.FlipDataHorizontally();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                    else if (y == 6 || y == 7)
                    {
                        otherTile.FlipDataVertically();
                        y = otherTile.Borders.IndexOf(sharedBorder);
                    }
                }
            }

        }

        public static void RotateData(List<List<char>> data)
        {
            for (int x = 0; x < data.Count / 2; x++)
            {
                for (int y = x; y < data.Count - x - 1; y++)
                {
                    char temp = data[x][y];
                    data[x][y] = data[y][data.Count - 1 - x];
                    data[y][data.Count - 1 - x] = data[data.Count - 1 - x][data.Count - 1 - y];
                    data[data.Count - 1 - x][data.Count - 1 - y] = data[data.Count - 1 - y][x];
                    data[data.Count - 1 - y][x] = temp;
                }
            }
        }

        private static List<Tile> PartOne(List<string> lines)
        {
            var tiles = new List<Tile>();
            Tile temp = null;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("Tile"))
                {
                    temp = new Tile
                    {
                        Id = int.Parse(lines[i].Replace("Tile ", "").Replace(":", "")),
                    };
                }
                else if (string.IsNullOrEmpty(lines[i]))
                {
                    tiles.Add(temp);
                }
                else
                {
                    temp.Data.Add(lines[i].ToList());
                }
            }
            tiles.Add(temp);

            foreach (var tile in tiles)
            {
                tile.GenerateBorders();
            }

            foreach (var tile in tiles)
            {
                foreach (var otherTile in tiles)
                {
                    if (tile.Id != otherTile.Id && otherTile.Borders.Any(b => tile.Borders.Contains(b)))
                    {
                        tile.MatchingSides.Add(otherTile);
                    }
                }
            }

            var corners = tiles.Where(t => t.MatchingSides.Count == 2).ToList();
            long result = 1;
            foreach (var corner in corners)
            {
                result *= corner.Id;
            }

            Console.WriteLine($"Solution 1: {result}");
            return tiles;
        }
    }


    public class Tile
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<string> Borders { get; set; }
        public List<Tile> MatchingSides { get; set; } = new List<Tile>();
        public List<List<char>> Data { get; set; } = new List<List<char>>();
        public List<List<char>> TrimmedData => Data.GetRange(1, Data.Count - 2).Select(d => d.GetRange(1, d.Count - 2)).ToList();

        public void GenerateBorders()
        {
            Borders = new List<string>();
            var topRow = string.Join("", Data[0]);
            var bottomRow = string.Join("", Data[Data.Count - 1]);
            var left = string.Join("", Data.Select(d => d.First()));
            var right = string.Join("", Data.Select(d => d.Last()));

            Borders.Add(topRow);
            Borders.Add(bottomRow);
            Borders.Add(left);
            Borders.Add(right);
            var reversed = Borders.Select(b => string.Join("", b.Reverse())).ToList();
            Borders.AddRange(reversed);
        }

        public void PrintData()
        {
            Console.WriteLine($"Tile: {Id} - ({X},{Y})");
            // Console.WriteLine(string.Join("\n", Data.Select(d => string.Join("", d)).ToList()));
            Console.WriteLine("\n");
        }

        public void FlipDataHorizontally()
        {
            Data.ForEach(d => d.Reverse());
            GenerateBorders();
        }

        public void FlipDataVertically()
        {
            Data.Reverse();
            GenerateBorders();
        }

        public void RotateData()
        {
            for (int x = 0; x < Data.Count / 2; x++)
            {
                for (int y = x; y < Data.Count - x - 1; y++)
                {
                    char temp = Data[x][y];
                    Data[x][y] = Data[y][Data.Count - 1 - x];
                    Data[y][Data.Count - 1 - x] = Data[Data.Count - 1 - x][Data.Count - 1 - y];
                    Data[Data.Count - 1 - x][Data.Count - 1 - y] = Data[Data.Count - 1 - y][x];
                    Data[Data.Count - 1 - y][x] = temp;
                }
            }
            GenerateBorders();
        }
    }

}
