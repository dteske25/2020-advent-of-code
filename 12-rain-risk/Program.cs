using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RainRisk
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\RainRisk\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            PartOne(lines);
            PartTwo(lines);
        }


        static void PartOne(List<string> lines)
        {
            var x = 0;
            var y = 0;
            var z = 0;


            foreach (var line in lines)
            {
                var direction = line.Substring(0, 1);
                var amount = int.Parse(line.Substring(1));

                if (direction == "R")
                {
                    z = (z - amount) % 360;
                }
                if (direction == "L")
                {
                    z = (z + amount) % 360;
                }
                if (direction == "F")
                {
                    var tempZ = ((z / 90) + 4) % 4;
                    if (tempZ == 0)
                    {
                        direction = "E";
                    }
                    if (tempZ == 1)
                    {
                        direction = "N";
                    }
                    if (tempZ == 2)
                    {
                        direction = "W";
                    }
                    if (tempZ == 3)
                    {
                        direction = "S";
                    }
                }

                if (direction == "N")
                {
                    y += amount;
                }
                if (direction == "S")
                {
                    y -= amount;
                }
                if (direction == "E")
                {
                    x += amount;
                }
                if (direction == "W")
                {
                    x -= amount;
                }


            }

            Console.WriteLine($"Solution 1: {Math.Abs(x) + Math.Abs(y)}");

        }


        static void PartTwo(List<string> lines)
        {
            var deltaX = 10;
            var deltaY = 1;
            var x = 0;
            var y = 0;

            foreach (var line in lines)
            {
                var direction = line.Substring(0, 1);
                var amount = int.Parse(line.Substring(1));
                if (direction == "R" || direction == "L")
                {
                    if (direction == "R")
                    {
                        amount = -amount;
                    }

                    var z = amount % 360;

                    if (z == 90 || z == -270)
                    {
                        var temp = deltaX;
                        deltaX = -deltaY;
                        deltaY = temp;
                    }
                    if (z == 180 || z == -180)
                    {
                        deltaX = -deltaX;
                        deltaY = -deltaY;
                    }

                    if (z == 270 || z == -90)
                    {
                        var temp = deltaX;
                        deltaX = deltaY;
                        deltaY = -temp;
                    }

                }
                
                if (direction == "F")
                {
                    x += deltaX * amount;
                    y += deltaY * amount;
                }

                if (direction == "N")
                {
                    deltaY += amount;
                }
                if (direction == "S")
                {
                    deltaY -= amount;
                }
                if (direction == "E")
                {
                    deltaX += amount;
                }
                if (direction == "W")
                {
                    deltaX -= amount;
                }
            }
            Console.WriteLine($"Solution 2: {Math.Abs(x) + Math.Abs(y)}");
        }

    }
}
