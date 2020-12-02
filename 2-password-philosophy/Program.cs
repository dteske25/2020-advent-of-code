using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordPhilosophy
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\PasswordPhilosophy\data.txt";
            var lines = File.ReadAllLines(filePath);
            var partOneValidPasswords = 0;
            foreach (var line in lines)
            {
                if (IsValidPasswordPartOne(line))
                {
                    partOneValidPasswords++;
                }
            }
            Console.WriteLine($"Valid Passwords, Pt 1: {partOneValidPasswords}");

            var partTwoValidPasswords = 0;
            foreach (var line in lines)
            {
                if (IsValidPasswordPartTwo(line))
                {
                    partTwoValidPasswords++;
                }
            }
            Console.WriteLine($"Valid Passwords, Pt 2: {partTwoValidPasswords}");
        }

        static bool IsValidPasswordPartOne(string line)
        {
            var regex = Regex.Match(line, "(\\d+)-(\\d+) ([a-zA-Z]): (.*)");
            var min = int.Parse(regex.Groups[1].Value);
            var max = int.Parse(regex.Groups[2].Value);
            var requiredChar = regex.Groups[3].Value;

            var password = regex.Groups[4].Value;

            var count = password.Count(p => requiredChar.Contains(p));
            return count >= min && count <= max;
        }

        static bool IsValidPasswordPartTwo(string line)
        {
            var regex = Regex.Match(line, "(\\d+)-(\\d+) ([a-zA-Z]): (.*)");
            var positionOne = int.Parse(regex.Groups[1].Value);
            var positionTwo = int.Parse(regex.Groups[2].Value);
            var requiredChar = char.Parse(regex.Groups[3].Value);

            var password = regex.Groups[4].Value;

            return password[positionOne-1] == requiredChar ^ password[positionTwo-1] == requiredChar;

        }
    }
}
