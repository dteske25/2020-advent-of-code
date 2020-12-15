using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DockingData
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\DockingData\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            PartOne(lines);
            PartTwo(lines);
        }

        private static void PartTwo(List<string> lines)
        {
            string mask = "";
            long maskOn = 0;

            var results = new Dictionary<long, long>();

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split("=")[1].Trim();
                    maskOn = Convert.ToInt64(mask.Replace("X", "0"), 2);
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[(?<addr>\d+)\] = (?<val>\d+)");
                    var address = long.Parse(match.Groups["addr"].Value);
                    

                    var val = long.Parse(match.Groups["val"].Value);

                    var maskedValue = address | maskOn;

                    var addresses = GeneratePossibleNumbers(mask, maskedValue);
                    foreach (var add in addresses)
                    {
                        if (!results.ContainsKey(add))
                        {
                            results.Add(add, val);
                        }
                        else
                        {
                            results[add] = val;
                        }
                    }
                   
                }
            }

            var solution = results.Sum(kvp => kvp.Value);
            Console.WriteLine($"Solution 2: {solution}");
        }

        static List<long> GeneratePossibleNumbers(string originalMask, long address)
        {
            var results = new List<long>();
            var indexes = originalMask.Select((c, i) => (c, i)).Where(c => c.c == 'X').ToList();
            for (int i = 0; i < Math.Pow(2, indexes.Count); i++)
            {

                var newValArr = address.ToBinary(originalMask.Length).ToArray();
                var bin = i.ToBinary(indexes.Count);
                for (int j = 0; j < indexes.Count; j++)
                {
                    newValArr[indexes[j].i] = bin[j];
                }
                var addr = Convert.ToInt64(string.Join("", newValArr), 2);
                results.Add(addr);
            }
            return results;
        }

        private static void PartOne(List<string> lines)
        {
            long maskOn = 0;
            long maskOff = 0;

            var results = new Dictionary<long, long>();

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    var maskVal = line.Split("=")[1].Trim();
                    maskOn = Convert.ToInt64(maskVal.Replace("X", "0"), 2);
                    maskOff = Convert.ToInt64(maskVal.Replace("X", "1"), 2);
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[(?<addr>\d+)\] = (?<val>\d+)");
                    var address = long.Parse(match.Groups["addr"].Value);
                    if (!results.ContainsKey(address))
                    {
                        results.Add(address, 0);
                    }

                    var val = long.Parse(match.Groups["val"].Value);

                    var maskedValue = (val | maskOn) & maskOff;
                   
                    results[address] = maskedValue;
                }
            }

            var solution = results.Sum(kvp => kvp.Value);
            Console.WriteLine($"Solution 1: {solution}");
        }

        public static string ToBinary(this long val, int padding = 0)
        {
            return Convert.ToString(val, 2).PadLeft(padding, '0');
        }

        public static string ToBinary(this int val, int padding = 0)
        {
            return Convert.ToString(val, 2).PadLeft(padding, '0');
        }
    }
}
