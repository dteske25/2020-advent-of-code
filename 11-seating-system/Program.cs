using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeatingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\SeatingSystem\data.txt";
            var lines = File.ReadAllLines(filePath).Select(c => c.ToList()).ToList();
            var linesCopy = lines.Select(l => l.ToList()).ToList();

            PartOne(lines);
            PartTwo(linesCopy);
        }

        static int GetAdjacentSeats(List<List<char>> lines, int x, int y)
        {
            var seats = 0;
            var xMin = Math.Max(x - 1, 0);
            var xMax = Math.Min(x + 1, lines.Count - 1);
            var yMin = Math.Max(y - 1, 0);
            var yMax = Math.Min(y + 1, lines.First().Count - 1);

            for (int tempX = xMin; tempX <= xMax; tempX++)
            {
                for (int tempY = yMin; tempY <= yMax; tempY++)
                {
                    if (tempX == x && tempY == y)
                    {
                        continue;
                    }

                    if (lines[tempX][tempY] == '#')
                    {
                        seats++;
                    }
                }
            }

            return seats;
        }

        static void PartOne(List<List<char>> lines)
        {
            var seatChanges = 0;

            while (true)
            {
                var seatsToFlip = new List<Tuple<int, int>>();
                for (int x = 0; x <= lines.Count - 1; x++)
                {
                    for (int y = 0; y <= lines.First().Count - 1; y++)
                    {
                        var seat = lines[x][y];

                        if (seat == '#')
                        {
                            if (GetAdjacentSeats(lines, x, y) >= 4)
                            {
                                seatsToFlip.Add(new Tuple<int, int>(x, y));
                            }
                        }
                        else if (seat == 'L')
                        {
                            if (GetAdjacentSeats(lines, x, y) == 0)
                            {
                                seatsToFlip.Add(new Tuple<int, int>(x, y));
                            }
                        }
                    }
                }

                foreach (var coord in seatsToFlip)
                {
                    var seat = lines[coord.Item1][coord.Item2];
                    lines[coord.Item1][coord.Item2] = seat == '#' ? 'L' : '#';
                }

                if (seatsToFlip.Any())
                {
                    seatChanges++;
                }
                else
                {
                    break;
                }
            }
            var occupied = lines.Sum(l => l.Count(c => c == '#'));
            var output = string.Join("\n", lines.Select(l => string.Join("", l)));

            Console.WriteLine($"Solution 1: {occupied}");

        }


        static char GetNextVisibleSeat(List<List<char>> lines, int x, int y, int deltaX, int deltaY)
        {
            if (x < 0 || x > lines.Count - 1 || y < 0 || y > lines.First().Count - 1)
            {
                // Out of bounds
                return '.';
            }
            if (lines[x][y] != '.')
            {
                return lines[x][y];
            }
            return GetNextVisibleSeat(lines, x + deltaX, y + deltaY, deltaX, deltaY);
        }

        static List<(int deltaX, int deltaY)> _directions = new List<(int, int)>
        {
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1)
        };
        static int GetVisibleSeats(List<List<char>> lines, int x, int y)
        {
            var seats = 0;
            foreach (var (deltaX, deltaY) in _directions)
            {
                if (GetNextVisibleSeat(lines, x + deltaX, y + deltaY, deltaX, deltaY) == '#')
                {
                    seats++;
                }
            }

            return seats;
        }

        static void PartTwo(List<List<char>> lines)
        {
            var seatChanges = 0;

            while (true)
            {
                var seatsToFlip = new List<(int, int)>();
                for (int x = 0; x <= lines.Count - 1; x++)
                {
                    for (int y = 0; y <= lines.First().Count - 1; y++)
                    {
                        var seat = lines[x][y];

                        if (seat == '#')
                        {
                            if (GetVisibleSeats(lines, x, y) >= 5)
                            {
                                seatsToFlip.Add((x, y));
                            }
                        }
                        else if (seat == 'L')
                        {
                            if (GetVisibleSeats(lines, x, y) == 0)
                            {
                                seatsToFlip.Add((x, y));
                            }
                        }
                    }
                }

                foreach (var (x, y) in seatsToFlip)
                {
                    lines[x][y] = lines[x][y] == '#' ? 'L' : '#';
                }

                if (seatsToFlip.Any())
                {
                    seatChanges++;
                }
                else
                {
                    break;
                }
            }
            var occupied = lines.Sum(l => l.Count(c => c == '#'));
            var output = string.Join("\n", lines.Select(l => string.Join("", l)));

            Console.WriteLine($"Solution 2: {occupied}");

        }
    }
}
