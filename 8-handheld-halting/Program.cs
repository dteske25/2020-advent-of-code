using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HandheldHalting
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\8-handheld-halting\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            var partOneAnswer = PartOne(lines);
            Console.WriteLine($"Solution 1: {partOneAnswer}");

            var partTwoAnswer = PartTwo(lines);
            Console.WriteLine($"Solution 2: {partTwoAnswer}");

        }

        static int PartOne(List<string> instructions)
        {
            var accumulator = 0;
            var currentIndex = 0;
            var visitedIndexes = new HashSet<int>();

            while (true)
            {
                var currInst = instructions[currentIndex];
                if (visitedIndexes.Contains(currentIndex))
                {
                    return accumulator;
                }
                else
                {
                    visitedIndexes.Add(currentIndex);
                }
                var tempVal = int.Parse(currInst.Split(" ")[1]);
                if (currInst.Contains("acc"))
                {
                    accumulator += tempVal;
                    currentIndex++;
                }
                else if (currInst.Contains("jmp"))
                {
                    currentIndex += tempVal;
                }
                else
                {
                    // no-op
                    currentIndex++;
                }
            }


        }


        static int PartTwo(List<string> instructions)
        {

            var accumulator = 0;
            var currentIndex = 0;
            var visitedIndexes = new HashSet<int>();

            while (true)
            {
                var currInst = instructions[currentIndex];
                if (visitedIndexes.Contains(currentIndex))
                {
                    break;
                }
                else
                {
                    visitedIndexes.Add(currentIndex);
                }
                var tempVal = int.Parse(currInst.Split(" ")[1]);
                if (currInst.Contains("acc"))
                {
                    accumulator += tempVal;
                    currentIndex++;
                }
                else if (currInst.Contains("jmp"))
                {
                    currentIndex += tempVal;
                }
                else
                {
                    // no-op
                    currentIndex++;
                }
            }
            // Just built up the potential indexes that can be switched
            foreach (var visitedIndex in visitedIndexes)
            {
                var changedInstruction = instructions[visitedIndex];
                // create a new list each time, otherwise bad shit will happen
                var updatedInstructions = new List<string>();
                updatedInstructions.AddRange(instructions);
                if (changedInstruction.Contains("jmp"))
                {
                    updatedInstructions[visitedIndex] = changedInstruction.Replace("jmp", "nop");
                }
                else if (changedInstruction.Contains("nop"))
                {
                    updatedInstructions[visitedIndex] = changedInstruction.Replace("nop", "jmp");
                }
                var result = FollowInstructionsToEnd(updatedInstructions);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }
            return 0;
        }

        static int? FollowInstructionsToEnd(List<string> instructions)
        {
            var accumulator = 0;
            var currentIndex = 0;
            var visitedIndexes = new HashSet<int>();

            while (true)
            {
                if (currentIndex == instructions.Count)
                {
                    return accumulator;
                }
                else if (visitedIndexes.Contains(currentIndex))
                {
                    return null;
                }
                else
                {
                    visitedIndexes.Add(currentIndex);
                }
                var currInst = instructions[currentIndex];
                var tempVal = int.Parse(currInst.Split(" ")[1]);
                if (currInst.Contains("acc"))
                {
                    accumulator += tempVal;
                    currentIndex++;
                }
                else if (currInst.Contains("jmp"))
                {
                    currentIndex += tempVal;
                }
                else
                {
                    // no-op
                    currentIndex++;
                }
            }
        }

    }
}
