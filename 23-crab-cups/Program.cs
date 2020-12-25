using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics;

namespace CrabCups
{
    static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\CrabCups\sample.txt";
            var line = File.ReadAllLines(filePath).First();

            PartOne(line);
            PartTwo(line);
        }

        private static void PartTwo(string line)
        {
            var cups = line.Select(t => int.Parse(t.ToString())).ToList();
            var cupsMax = cups.Max();
            var list = new LinkedListNode<int> { Value = cups[0] };
            var currentCup = list;
            for (var i = 1; i < cups.Count; i++)
            {
                currentCup.Next = new LinkedListNode<int> { Value = cups[i] };
                currentCup = currentCup.Next;
            }
            for (var i = cupsMax + 1; i < 1_000_000; i++)
            {
                currentCup.Next = new LinkedListNode<int> { Value = i };
                currentCup = currentCup.Next;
            }
            currentCup.Next = list;

            currentCup = currentCup.Next;
            for (int i = 0; i < 10_000_000; i++)
            {
                Console.Write($"\rProgress: {i}");

                var startingCupToRemove = currentCup.Next;
                var excludedValues = new List<int> { startingCupToRemove.Value, startingCupToRemove.Next.Value, startingCupToRemove.Next.Next.Value };
                currentCup.Next = startingCupToRemove.Next.Next.Next;

                var nextFound = false;
                var targetCupValue = currentCup.Value - 1;
                while (!nextFound)
                {
                    targetCupValue = Euclid.Modulus(targetCupValue, cupsMax);
                    if (targetCupValue == 0)
                    {
                        targetCupValue = cupsMax;
                    }
                    if (!excludedValues.Contains(targetCupValue))
                    {
                        nextFound = true;
                    }
                    else
                    {
                        targetCupValue--;
                    }
                }
                var targetNode = FindNode(currentCup, targetCupValue);
                startingCupToRemove.Next.Next.Next = targetNode.Next;
                targetNode.Next = startingCupToRemove;
                currentCup = currentCup.Next;
            }

            var start = FindNode(list, 1);
            Console.WriteLine($"Solution 2: {start.Next.Value * start.Next.Next.Value}");
        }

        private static void PartOne(string line)
        {
            var cups = line.Select(t => int.Parse(t.ToString())).ToList();
            var cupsMax = cups.Max();
            var currentCup = cups.First();
            var currentCupIndex = 0;
            for (int i = 0; i < 10; i++)
            {
                var targetCupIndex = 0;
                var targetCupValue = cups[currentCupIndex] - 1;
                var rangeIndex = Euclid.Modulus(currentCupIndex + 1, cups.Count);
                var excludedCups = cups.GetRange(rangeIndex, Math.Min(3, cups.Count - rangeIndex));
                excludedCups.AddRange(cups.GetRange(0, 3 - excludedCups.Count));
                cups.RemoveAll(c => excludedCups.Contains(c));

                var nextFound = false;
                while (!nextFound)
                {
                    targetCupValue = Euclid.Modulus(targetCupValue, cupsMax);
                    if (targetCupValue == 0)
                    {
                        targetCupValue = cupsMax;
                    }
                    if (!excludedCups.Contains(targetCupValue))
                    {
                        targetCupIndex = cups.IndexOf(targetCupValue);
                        nextFound = true;
                    }
                    else
                    {
                        targetCupValue--;
                    }
                }
                cups.InsertRange(targetCupIndex + 1, excludedCups);
                currentCupIndex = Euclid.Modulus(cups.IndexOf(currentCup) + 1, cups.Count);
                currentCup = cups[currentCupIndex];
            }

            var startingIndex = Euclid.Modulus(cups.IndexOf(1), cups.Count);
            var resultingList = RotateList(cups, startingIndex).GetRange(1, cups.Count - 1);

            Console.WriteLine($"Solution 1: {string.Join("", resultingList)}");
        }

        static List<int> RotateList(List<int> startingList, int newStart)
        {
            var newList = new List<int>();
            var endRange = startingList.GetRange(newStart, startingList.Count - newStart);
            newList.AddRange(endRange);
            newList.AddRange(startingList.GetRange(0, startingList.Count - newList.Count));
            return newList;
        }


        static LinkedListNode<int> FindNode(LinkedListNode<int> node, int val)
        {
            var currentNode = node;
            while (currentNode.Value != val)
            {
                currentNode = currentNode.Next;
            }
            return currentNode;
        }

        static void PrintList(LinkedListNode<int> node)
        {
            var currentNode = node;
            var visited = new HashSet<int>();
            while (!visited.Contains(currentNode.Value))
            {
                visited.Add(currentNode.Value);
                Console.Write($"{currentNode.Value},");
                currentNode = currentNode.Next;
            }
            Console.WriteLine();
        }
    }

    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; set; }
    }
}
