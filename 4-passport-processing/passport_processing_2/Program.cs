using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PassportProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\4-passport-processing\data.txt";
            var lines = File.ReadAllLines(filePath);
            var keys = new[] { @"byr\:(?<byr>\d+)", @"iyr\:(?<iyr>\d+)", @"eyr\:(?<eyr>\d+)", @"hgt\:(?<hgt>\d+cm|\d+in)", @"hcl\:(?<hcl>#[0-9a-f]+)", @"ecl\:(?<ecl>amb|blu|brn|gry|grn|hzl|oth)", @"pid\:(?<pid>\d+)" };


            StringBuilder sb = new StringBuilder();
            var passports = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    passports.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append($" {line}");
                }
            }
            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                passports.Add(sb.ToString());
            }
            var filteredPassports = passports.Where(p => keys.All(k => Regex.IsMatch(p, k)));
            var validatedCount = 0;
            foreach(var passport in filteredPassports)
            {
                var valid = true;
                var matches = keys.Select(k => Regex.Match(passport, k));
                foreach (var match in matches)
                {
                    if (!valid)
                    {
                        break;
                    }
                    if (match.Groups["byr"].Success)
                    {
                        valid = ValidateRange(match.Groups["byr"].Value, 1920, 2002);
                        continue;
                    }
                    if (match.Groups["iyr"].Success)
                    {
                        valid = ValidateRange(match.Groups["iyr"].Value, 2010, 2020);
                        continue;
                    }
                    if (match.Groups["eyr"].Success)
                    {
                        valid = ValidateRange(match.Groups["eyr"].Value, 2020, 2030);
                        continue;
                    }
                    if (match.Groups["hgt"].Success)
                    {
                        var hgtStr = match.Groups["hgt"].Value;
                        if (hgtStr.Contains("cm"))
                        {
                            valid = ValidateRange(hgtStr.Replace("cm", ""), 150, 193);
                        }
                        else
                        {
                            valid = ValidateRange(hgtStr.Replace("in", ""), 59, 76);
                        }
                        continue;
                    }
                    if (match.Groups["hcl"].Success)
                    {
                        // #FFFFFF
                        valid = match.Groups["hcl"].Value.Length == 7;
                        continue;
                    }
                    if (match.Groups["pid"].Success)
                    {
                        valid = match.Groups["pid"].Value.Length == 9;
                        continue;
                    }                    
                }
                if (valid)
                {
                    validatedCount++;
                }
            }

            Console.WriteLine($"Fully Validated Count: {validatedCount}");
        }

        

        static bool ValidateRange(string unparsedValue, int startIncl, int endIncl)
        {
            var value = int.Parse(unparsedValue);
            return value >= startIncl && value <= endIncl;
        }
    }
}
