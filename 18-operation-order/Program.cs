using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OperationOrder
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\OperationOrder\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();
            PartOne(lines);
            PartTwo(lines);
        }

        private static void PartTwo(List<string> lines)
        {
            long sum = 0;
            foreach (var line in lines)
            {
                var temp = CalculateExpressionPartTwo(line);
                sum += temp;
            }
            Console.WriteLine($"Solution 2: {sum}");
        }

        private static long CalculateExpressionPartTwo(string expr)
        {
            long exprValue = 0;
            var operation = '+';
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '(')
                {
                    var indStart = ++i;
                    var groups = 1;
                    while (i < expr.Length)
                    {
                        if (expr[i] == '(')
                        {
                            groups++;
                        }
                        else if (expr[i] == ')')
                        {
                            groups--;
                        }

                        if (groups == 0)
                        {

                            exprValue = GetValue(exprValue, operation, CalculateExpressionPartTwo(expr.Substring(indStart, i - indStart)));
                            break;
                        }
                        i++;
                    }
                }
                else if (char.IsDigit(expr[i]))
                {
                    exprValue = GetValue(exprValue, operation, long.Parse(expr[i].ToString()));
                }
                else if (expr[i] == '*')
                {
                    var subStr = expr.Substring(i + 1);
                    exprValue = GetValue(exprValue, '*', CalculateExpressionPartTwo(subStr));
                    i += subStr.Length;
                }
                else if (expr[i] == '+')
                {
                    operation = expr[i];
                }
            }
            return exprValue;
        }



        private static void PartOne(List<string> lines)
        {
            long sum = 0;
            foreach (var line in lines)
            {
                var temp = CalculateExpression(line);
                sum += temp;
            }
            // wrong: 219107660
            Console.WriteLine($"Solution 1: {sum}");
        }


        private static long CalculateExpression(string expr)
        {
            long exprValue = 0;
            var operation = '?';
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '(')
                {
                    var indStart = ++i;
                    var groups = 1;
                    while (i < expr.Length)
                    {
                        if (expr[i] == '(')
                        {
                            groups++;
                        }
                        else if (expr[i] == ')')
                        {
                            groups--;
                        }

                        if (groups == 0)
                        {

                            exprValue = GetValue(exprValue, operation, CalculateExpression(expr.Substring(indStart, i - indStart)));
                            break;
                        }
                        i++;
                    }
                }
                else if (char.IsDigit(expr[i]))
                {
                    exprValue = GetValue(exprValue, operation, long.Parse(expr[i].ToString()));
                }
                else if (expr[i] == '*' || expr[i] == '+')
                {
                    operation = expr[i];
                }
            }
            return exprValue;
        }

        private static long GetValue(long left, char operation, long right)
        {
            if (operation == '+')
            {
                return left + right;
            }
            else if (operation == '*')
            {
                return left * right;
            }
            else
            {
                return right;
            }

        }
    }
}
