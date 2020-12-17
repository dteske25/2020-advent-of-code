using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicketTranslation
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\TicketTranslation\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            var rules = new Dictionary<int, List<string>>();
            List<int> yourTicket = new List<int>();
            List<List<int>> nearbyTickets = new List<List<int>>();
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("your ticket"))
                {
                    yourTicket = lines[++i].Split(',').Select(i => int.Parse(i)).ToList();

                    continue;
                }
                else if (line.StartsWith("nearby tickets"))
                {
                    i++;
                    while (i < lines.Count)
                    {
                        line = lines[i];
                        nearbyTickets.Add(line.Split(',').Select(i => int.Parse(i)).ToList());
                        i++;
                    }
                    break;
                }
                else
                {
                    var match = Regex.Match(line, @"(?<ruleName>.*): (?<range1>\d+-\d+) or (?<range2>\d+-\d+)");
                    if (match.Groups["ruleName"].Success)
                    {
                        var rangeOne = match.Groups["range1"].Value.Split("-").Select(i => int.Parse(i));
                        var rangeTwo = match.Groups["range2"].Value.Split("-").Select(i => int.Parse(i));
                        var vals = Enumerable.Range(rangeOne.First(), rangeOne.Last() - rangeOne.First() + 1).ToList();
                        vals.AddRange(Enumerable.Range(rangeTwo.First(), rangeTwo.Last() - rangeTwo.First() + 1));
                        vals.Sort();

                        foreach (var val in vals)
                        {
                            if (!rules.ContainsKey(val))
                            {
                                rules.Add(val, new List<string>());
                            }
                            rules[val].Add(match.Groups["ruleName"].Value);
                        }
                    }
                }
            }

            var validTickets = PartOne(rules, nearbyTickets);
            PartTwo(rules, validTickets, yourTicket);
        }
        private static List<List<int>> PartOne(Dictionary<int, List<string>> rules, List<List<int>> nearbyTickets)
        {
            var invalidValues = new List<int>();
            var validTickets = new List<List<int>>();
            foreach (var ticket in nearbyTickets)
            {
                if (ticket.All(t => rules.ContainsKey(t)))
                {
                    validTickets.Add(ticket);
                }
                invalidValues.AddRange(ticket.Where(t => !rules.ContainsKey(t)));
            }

            var result = invalidValues.Sum();
            Console.WriteLine($"Solution 1: {result}");
            return validTickets;
        }

        private static void PartTwo(Dictionary<int, List<string>> rules, List<List<int>> validTickets, List<int> yourTicket)
        {
            var indexMappings = new Dictionary<string, List<int>>();
            var usedPossibleValues = new HashSet<string>();
            for (int i = 0; i < yourTicket.Count; i++)
            {
                var allValues = validTickets.Select(a => a[i]).Distinct();
                var possibleValues = rules[yourTicket[i]].ToHashSet();
                possibleValues.ExceptWith(usedPossibleValues);
                foreach (var value in allValues)
                {
                    // for each value at index i, intersect the possible values
                    possibleValues.IntersectWith(rules[value]);
                    // if at any point there's only one, we're done searching, make sure it isn't used again
                    if (possibleValues.Count == 1)
                    {
                        var knownValue = possibleValues.First();
                        usedPossibleValues.Add(knownValue);
                        if (indexMappings.ContainsKey(knownValue))
                        {
                            // clear out any existing values since we know for sure it's this one
                            indexMappings[knownValue] = new List<int>();
                        }
                        break;
                    }
                    
                }
                // for any possible mappings left in the set, add the current index
                foreach (var possibleMapping in possibleValues)
                {
                    if (!indexMappings.ContainsKey(possibleMapping))
                    {
                        indexMappings.Add(possibleMapping, new List<int>());
                    }
                    indexMappings[possibleMapping].Add(i);
                }
            }

            var finalMappings = new Dictionary<string, int>();
            // starting with the ones that only have one possible value, assign and
            // remove them from the list of possible values on the other keys.
            // If this works, we'll be able to make it through the whole list this way
            var results = indexMappings.OrderBy(kvp => kvp.Value.Count).ToList();
            foreach (var kvp in results)
            {
                if (kvp.Value.Count == 1)
                {
                    var val = kvp.Value.First();
                    finalMappings.Add(kvp.Key, val);
                    foreach (var pair in results)
                    {
                        pair.Value.Remove(val);
                    }
                }
            }

            // now find the departure ones and multiply them together
            double result = 1;
            foreach (var dept in finalMappings.Where(kvp => kvp.Key.StartsWith("departure")))
            {
                result *= yourTicket[dept.Value];
            }
            Console.WriteLine($"Solution 2: {result}");
        }
    }
}
