using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RambunctiousRecitation
{
    static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\RambunctiousRecitation\data.txt";
            var line = File.ReadAllLines(filePath).First();

            PartOne(line);
            PartTwo(line);
        }

        private static void PartTwo(string line)
        {
            var numbers = line.Split(',').Select(c => int.Parse(c)).ToList();
            var mostRecentNumber = numbers[0];
            var lastTwoIndexes = new Dictionary<int, Queue<int>>();
            var turns = 0;
            while (turns < 30_000_000)
            {
                if (turns < numbers.Count())
                {
                    mostRecentNumber = numbers[turns];
                }
                else
                {
                    if (lastTwoIndexes.ContainsKey(mostRecentNumber))
                    {
                        if (lastTwoIndexes[mostRecentNumber].Count == 2)
                        {
                            var indexes = lastTwoIndexes[mostRecentNumber].ToList();
                            mostRecentNumber = indexes.Last() - indexes.First();
                        }
                        else
                        {
                            mostRecentNumber = 0;
                        }
                    }
                    else
                    {
                        mostRecentNumber = 0;
                    }
                    
                }
                if (!lastTwoIndexes.ContainsKey(mostRecentNumber))
                {
                    lastTwoIndexes.Add(mostRecentNumber, new Queue<int>(2));
                }
                lastTwoIndexes[mostRecentNumber].Enqueue(turns);
                if (lastTwoIndexes[mostRecentNumber].Count > 2)
                {
                    lastTwoIndexes[mostRecentNumber].Dequeue();
                }
                turns++;
            }

            Console.WriteLine($"Solution 2: {mostRecentNumber}");
        }


        public static List<int> FindAllIndexes(this List<int> list, int value)
        {
            var indexes = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        private static void PartOne(string line)
        {
            var numbers = line.Split(',').Select(c => int.Parse(c)).ToList();
            var list = new List<int>();
            var numberSet = new HashSet<int>();
            var turns = 0;
            while (turns < 2020)
            {
                if (turns < numbers.Count())
                {
                    list.Add(numbers[turns]);
                }
                else
                {
                    var num = list.Last();
                    if (numberSet.Contains(num))
                    {
                        var indexes = list.FindAllIndexes(num);

                        if (indexes.Count >= 2)
                        {
                            var mostRecentIndex = indexes[indexes.Count - 1];
                            var secondMostRecentIndex = indexes[indexes.Count - 2];

                            var newNum = mostRecentIndex - secondMostRecentIndex;
                            list.Add(newNum);
                            numberSet.Add(newNum);
                        }
                        else
                        {
                            list.Add(0);
                            numberSet.Add(0);
                        }
                    }
                    else
                    {
                        list.Add(0);
                        numberSet.Add(0);
                    }
                }
                turns++;
            }

            Console.WriteLine($"Solution 1: {list.Last()}");
        }
    }
}
