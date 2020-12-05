using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryBoarding
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\5-binary-boarding\BinaryBoarding\data.txt";
            var lines = File.ReadAllLines(filePath);

            var results = new List<int>();
            foreach (var row in lines)
            {
                results.Add(CalculateRow(row));
            }
            results.Sort();

            Console.WriteLine($"Solution 1: {results.Last()}");
            
            for (int i = 0; i < results.Count - 1; i++)
            {
                if (i + 1 < results.Count)
                {
                    if (results[i + 1] - results[i] == 2)
                    {
                        Console.WriteLine($"Solution 2: {results[i] + 1}");
                    }
                }
            }

        }

        static int CalculateRow(string str)
        {

            var lowerRowBound = 0;
            var upperRowBound = 127;

            var lowerColBound = 0;
            var upperColBound = 7;

            foreach (char c in str)
            {
                var deltaR = (int)Math.Floor(((double)upperRowBound - lowerRowBound + 1) / 2);
                var deltaC = (int)Math.Floor(((double)upperColBound - lowerColBound + 1) / 2);
                if (c == 'F')
                {
                    upperRowBound -= deltaR;
                }
                if (c == 'B')
                {
                    lowerRowBound += deltaR;
                }
                if (c == 'L')
                {
                    upperColBound -= deltaC;
                }
                if (c == 'R')
                {
                    lowerColBound += deltaC;
                }
            }

            return lowerRowBound * 8 + lowerColBound;
        }
    }
}
