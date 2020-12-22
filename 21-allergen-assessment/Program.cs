using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AllergenAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\AllergenAssessment\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();
            var map = PartOne(lines);
            PartTwo(map);
        }

        private static void PartTwo(Dictionary<string, HashSet<string>> map)
        {
            var minOrdering = map.OrderBy(kvp => kvp.Value.Count).ToList();
            var solution = new Dictionary<string, string>();
            var usedItems = new HashSet<string>();

            while (map.Count > 0)
            {
                var sorted = map.OrderBy(kvp => kvp.Value.Count);
                var currentItem = sorted.First();
                if (currentItem.Value.Count == 1)
                {
                    solution.Add(currentItem.Key, currentItem.Value.First());
                    map.Remove(currentItem.Key);
                    foreach (var remainingKey in map.Keys)
                    {
                        map[remainingKey].Remove(currentItem.Value.First());
                    }
                }
            }

            var alphabeticalOrder = solution.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
            var result = string.Join(",", alphabeticalOrder);

            Console.WriteLine($"Solution 2: {result}");
        }

        private static Dictionary<string, HashSet<string>> PartOne(List<string> lines)
        {
            var allergenMap = new Dictionary<string, HashSet<string>>();
            var allFoods = new List<string>();
            foreach (var line in lines)
            {
                var matches = Regex.Match(line, @"(?<ingredients>.*)\(contains (?<allergens>.*)\)");
                var allergens = matches.Groups["allergens"].Value.Split(",").Select(s => s.Trim()).ToList();
                var foods = matches.Groups["ingredients"].Value.Split(" ").Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                allFoods.AddRange(foods);
                foreach (var allergen in allergens)
                {
                    if (allergenMap.ContainsKey(allergen))
                    {
                        allergenMap[allergen].IntersectWith(foods);
                    }
                    else
                    {
                        allergenMap.Add(allergen, foods.ToHashSet());
                    }
                }
            }
            var allergenFoods = allergenMap.Values.SelectMany(s => s).ToHashSet();
            var result = allFoods.Where(f => !allergenFoods.Contains(f)).Count();

            Console.WriteLine($"Solution 1: {result}");
            return allergenMap;
        }
    }
}
