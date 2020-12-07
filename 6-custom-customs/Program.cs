using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomCustoms
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\6-custom-customs\data.txt";
            var lines = File.ReadAllLines(filePath);

            var results = new List<int>();
            var temp = new HashSet<char>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    results.Add(temp.Count());
                    temp.Clear();
                }
                else
                {
                    foreach(var c in line)
                    {
                        temp.Add(c);
                    }
                }
            }
            results.Add(temp.Count());
            temp.Clear();
            var finalResult = results.Sum();
            Console.WriteLine($"Solution 1: {finalResult}");

            // Should have done this for the first time
            var list = new List<List<string>>();
            var tempList = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    list.Add(tempList);
                    tempList = new List<string>();
                }
                else
                {
                    tempList.Add(line);
                }
            }
            list.Add(tempList);

            var resultsAlso = new List<int>();
            foreach(var group in list)
            {
                var allSameCount = 0;
                var allQuestions = string.Join("", group).Distinct();
                foreach (var c in allQuestions)
                {
                    if (group.All(g => g.Contains(c)))
                    {
                        allSameCount++;
                    }
                }
                resultsAlso.Add(allSameCount);
            }

            var finalResultAlso = resultsAlso.Sum();
            Console.WriteLine($"Solution 2: {finalResultAlso}");
        }
    }
}
