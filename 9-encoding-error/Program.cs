using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EncodingError
{
    class Program
    {
        static void Main(string[] args)
        {

            var filePath = @"R:\Code\2020-advent-of-code\9-encoding-error\data.txt";
            var lines = File.ReadAllLines(filePath).Select(c => long.Parse(c)).ToList();

            var partOne = PartOne(lines, 25);
            Console.WriteLine($"Solution 1: {partOne}");

            var partTwo = PartTwo(lines, partOne);
            Console.WriteLine($"Solution 2: {partTwo}");
        }

        static bool CheckSums(List<long> input, long value)
        {
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    if (input[i] + input[j] == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static long PartOne(List<long> input, int capacity)
        {
            var numberBuffer = input.GetRange(0, capacity).ToList();
            var index = capacity;
            while (index < input.Count)
            {
                if (!CheckSums(numberBuffer, input[index]))
                {
                    return input[index];
                }
                index++;
                numberBuffer = input.GetRange(index - capacity, capacity).ToList();
            }
            return 0;

        }

        static long PartTwo(List<long> input, long goal)
        {
            var startingIndex = 0;
            var endingIndex = 0;
            long currentSum = 0;
            while (true)
            {
                if (currentSum < goal)
                {
                    endingIndex++;
                    currentSum += input[endingIndex];
                }
                else if (currentSum > goal)
                {
                    startingIndex++;
                    endingIndex = startingIndex;
                    currentSum = input[startingIndex];
                }
                else
                {
                    var range = input.GetRange(startingIndex, endingIndex - startingIndex);
                    return range.Min() + range.Max();
                }
            }

        }
    }
}
