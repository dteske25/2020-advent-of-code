using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandyHaversacks
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\7-handy-haversacks\data.txt";
            var lines = File.ReadAllLines(filePath);
            var rules = new Dictionary<string, List<string>>();

            foreach (var line in lines)
            {
                var tokens = line.Split("contain").Select(s => s.Trim()).ToList();
                if (rules.ContainsKey(tokens[0]))
                {
                    rules[tokens[0]].Add(tokens[1]);
                }
                else
                {
                    rules.Add(tokens[0], new List<string>() { tokens[1] });
                }
            }


            var searchValue = "shiny gold";
            var potentialBags = SearchForBags(searchValue, rules);
            Console.WriteLine($"Solution 1: {potentialBags.Count}");

            var requiredBags = CountBags(searchValue, rules);
            Console.WriteLine($"Solution 2: {requiredBags}");
        }

        static HashSet<string> SearchForBags(string searchValue, Dictionary<string, List<string>> rules)
        {
            var potentialBags = new HashSet<string>();
            foreach (var kvp in rules)
            {
                if (kvp.Value.Any(v => v.Contains(searchValue)))
                {

                    potentialBags.Add(kvp.Key);
                    potentialBags.UnionWith(SearchForBags(kvp.Key.Replace("bags", "bag"), rules));

                }
            }
            return potentialBags;
        }


        static int CountBags(string searchValue, Dictionary<string, List<string>> rules)
        {
            var bagCount = 0;
            foreach (var kvp in rules)
            {
                if (kvp.Key.Contains(searchValue))
                {
                    foreach (var value in kvp.Value)
                    {
                        var bagTokens = value.Split(",");
                        foreach(var token in bagTokens)
                        {
                            var match = Regex.Match(token, @"(\d+) (.*)");
                            
                            if (match.Groups[1].Success)
                            {
                                var tempCount = int.Parse(match.Groups[1].Value);
                                if (match.Groups[2].Success && tempCount > 0)
                                {
                                    var bagType = match.Groups[2].Value.Replace(".", "").Replace("bags", "").Replace("bag", "").Trim();
                                    bagCount += tempCount * (CountBags(bagType, rules) + 1);
                                }   
                            }
                        }
                    }
                }
            }
            return bagCount;
        }
    }
}
