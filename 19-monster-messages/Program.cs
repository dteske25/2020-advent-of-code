using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Something
{
    static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\19-monster-messages\data_1.txt";
            var lines = File.ReadAllLines(filePath).ToList();
            PartOne(lines);
            filePath = @"R:\Code\2020-advent-of-code\19-monster-messages\part_2\data_input.txt";
            lines = File.ReadAllLines(filePath).ToList();
            PartTwo(lines);
        }



        private static void PartTwo(List<string> lines)
        {
            var result = 0;
            foreach (var input in lines)
            {
                AntlrInputStream inputStream = new AntlrInputStream(input);
                var lexer = new MonsterMessagesLexer(inputStream);
               
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                var output = new StringWriter();
                var error = new StringWriter();
                var parser = new MonsterMessagesParser(commonTokenStream, output, error);
                var messageRule = parser.message();
                var visitor = new MonsterMessagesVisitor();
                visitor.VisitMessage(messageRule);
                if (string.IsNullOrEmpty(error.ToString()))
                {
                    result++;
                }
            }
            Console.WriteLine($"Solution 2: {result}");
        }



        #region Part One

        private static void PartOne(List<string> lines)
        {
            var result = 0;
            var rules = lines.Where(l => Regex.IsMatch(l, @"\d+:.*")).ToDictionary(l => l.Split(":")[0], l => l.Split(":")[1].Trim());
            var inputs = lines.Where(l => Regex.IsMatch(l, @"^(a|b)+")).ToList();
            var possibleInputs = new Dictionary<string, List<string>>();
            BuildPossibilities(rules, possibleInputs, "0");
            var allPossibilities = possibleInputs["0"].ToHashSet();
            foreach (var input in inputs)
            {
                if (allPossibilities.Contains(input))
                {
                    result++;
                }
            }

            Console.WriteLine($"Solution 1: {result}");
        }

        private static List<string> BuildPossibilities(Dictionary<string, string> rules, Dictionary<string, List<string>> possibilities, string ruleIndex)
        {
            if (possibilities.ContainsKey(ruleIndex))
            {
                return possibilities[ruleIndex];
            }
            else
            {
                var possibleStrings = new List<string>();
                var tokens = rules[ruleIndex].Split('|').Select(t => t.Trim().Replace("\"", ""));
                foreach (var token in tokens)
                {
                    if (token == "a" || token == "b")
                    {
                        possibleStrings.Add(token);
                    }
                    else
                    {
                        var ruleNumbers = token.Split(" ");
                        var allRuleLists = ruleNumbers.Select(r => BuildPossibilities(rules, possibilities, r)).ToList();
                        possibleStrings.AddRange(GetPermutations(allRuleLists).ToList());

                    }
                }
                if (possibilities.ContainsKey(ruleIndex))
                {
                    possibilities[ruleIndex].AddRange(possibleStrings);
                }
                else
                {
                    possibilities.Add(ruleIndex, possibleStrings);
                }
                return possibleStrings;
            }
        }

        private static IEnumerable<string> GetPermutations(IEnumerable<IEnumerable<string>> allRuleLists)
        {
            if (allRuleLists.Count() == 1) 
            {
                return allRuleLists.First();
            }
            else
            {
                var result = new List<string>();
                foreach (var str in allRuleLists.First())
                {
                    var permutations = GetPermutations(allRuleLists.Skip(1)).ToList();
                    foreach (var perm in permutations)
                    {
                        result.Add($"{str}{perm}");
                    }
                }
                return result;
            }
        }

        #endregion
    }
    

    public class MonsterMessagesVisitor : MonsterMessagesBaseVisitor<object>
    {
        public bool IsValid { get; set; }

        public override object VisitMessage([NotNull] MonsterMessagesParser.MessageContext context)
        {
            var baseRule = context.r0();
            IsValid = !baseRule.IsEmpty;
            return base.VisitMessage(context);
        }
    }
}
