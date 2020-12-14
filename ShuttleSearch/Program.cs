using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace ShuttleSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\ShuttleSearch\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            PartOne(lines);
            PartTwo(lines);
        }

        static void PartOne(List<string> lines)
        {
            var startingTimestamp = int.Parse(lines[0]);
            var busses = lines[1].Split(",").Where(i => i != "x").Select(i => int.Parse(i));

            var currentTimestamp = startingTimestamp;
            int? foundBus = null;
            while (foundBus == null)
            {
                foreach (var bus in busses)
                {
                    if (currentTimestamp % bus == 0)
                    {
                        foundBus = bus;
                        break;
                    }
                }
                if (foundBus == null)
                {
                    currentTimestamp++;
                }
            }

            var result = (currentTimestamp - startingTimestamp) * foundBus.Value;
            Console.WriteLine($"Solution 1: {result}");

        }

        // This was my brute force attempt, didn't work though
        //static void PartTwo(List<string> lines)
        //{
        //    var tokens = lines[1].Split(",");
        //    var pairs = new List<(int, int)>();
        //    for (int i = 0; i < tokens.Length; i++)
        //    {
        //        if (tokens[i] != "x")
        //        {
        //            pairs.Add((int.Parse(tokens[i]), i));
        //        }
        //    }
        //    pairs = pairs.OrderBy(pairs => pairs.Item1).ToList();

        //    long? foundTimestamp = null;
        //    long currentTimestamp = 0;
        //    long loop = 1;
        //    var (maxId, maxIdOffset) = pairs.Last();
        //    while (foundTimestamp == null)
        //    {
        //        var allValid = true;
        //        foreach (var (id, offset) in pairs)
        //        {
        //            if ((currentTimestamp + offset) % id != 0)
        //            {
        //                allValid = false;
        //                break;
        //            }
        //        }

        //        if (!allValid)
        //        {
        //            currentTimestamp = (maxId * loop - maxIdOffset);
        //            loop++;
        //        }
        //        else
        //        {
        //            foundTimestamp = currentTimestamp;
        //        }
        //    }

        //    Console.WriteLine($"Solution 2: {foundTimestamp.Value}");
        //}

        static void PartTwo(List<string> lines)
        {
            var tokens = lines[1].Split(",");
            var pairs = new List<(int, int)>();
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] != "x")
                {
                    pairs.Add((int.Parse(tokens[i]), i));
                }
            }

            // Worked through this with everyone, not sure I fully understand the math yet
            long stepSize = 1;
            long answer = 1;
            foreach (var entry in pairs)
            {
                while ((answer + entry.Item2) % entry.Item1 != 0)
                {
                    answer += stepSize;
                }
                stepSize *= entry.Item1;
            }

            Console.WriteLine("I'm a baboon - wasn't able to solve on my own.");
            Console.WriteLine($"Solution 2: {answer}");
        }
    }
}
