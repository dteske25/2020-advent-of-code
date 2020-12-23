using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrabCombat
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\CrabCombat\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            var originalHands = PartOne(lines);
            PartTwo(originalHands);
        }

        private static void PartTwo(List<Queue<int>> originalHands)
        {
            var playerOne = originalHands[0];
            var playerTwo = originalHands[1];
            var playedHands = new HashSet<string>();

            var winner = PlayGame(playerOne, playerTwo, out var infiniteWinner);
            long result;
            if (winner == "p1")
            {
                result = CalculateWinningHand(playerOne);
            }
            else
            {
                result = CalculateWinningHand(playerTwo);
            }
            // 7696 is too low
            Console.WriteLine($"Solution 2: {result}");
        }

        public static long CalculateWinningHand(Queue<int> winningPlayer)
        {
            long result = 0;
            int multiplyer = winningPlayer.Count;
            while (winningPlayer.Count > 0)
            {
                result += winningPlayer.Dequeue() * multiplyer;
                multiplyer--;
            }
            return result;
        }

        public static string PlayGame(Queue<int> playerOne, Queue<int> playerTwo, out bool winnerByRecursion)
        {
            var hashSet = new HashSet<string>();
            while (playerOne.Count > 0 && playerTwo.Count > 0)
            {
                var hash = string.Join(",", playerOne.ToArray()) + "-" + string.Join(",", playerTwo.ToArray());
                if (hashSet.Contains(hash))
                {
                    winnerByRecursion = true;
                    return "p1";
                }

                var p1Card = playerOne.Dequeue();
                var p2Card = playerTwo.Dequeue();
                var result = "";

                if (playerOne.Count >= p1Card && playerTwo.Count >= p2Card)
                {
                    result = PlayGame(new Queue<int>(playerOne.ToArray().Take(p1Card)), new Queue<int>(playerTwo.ToArray().Take(p2Card)), out var recursionResult);
                    //if (recursionResult)
                    //{
                    //    winnerByRecursion = true;
                    //    return result;
                    //}
                }
                else if (p1Card > p2Card)
                {
                    result = "p1";

                }
                else if (p1Card < p2Card)
                {
                    result = "p2";
                }
                if (result == "p1")
                {
                    playerOne.Enqueue(p1Card);
                    playerOne.Enqueue(p2Card);
                }
                else
                {
                    playerTwo.Enqueue(p2Card);
                    playerTwo.Enqueue(p1Card);
                }
                hashSet.Add(hash);
            }
            winnerByRecursion = false;
            return playerTwo.Count == 0 ? "p1" : "p2";
        }

        private static List<Queue<int>> PartOne(List<string> lines)
        {
            var playerOne = new Queue<int>();
            var playerTwo = new Queue<int>();

            var count = 1;
            var addingToPlayerTwo = false;
            while (count < lines.Count)
            {
                if (string.IsNullOrWhiteSpace(lines[count]))
                {
                    count++;
                    continue;
                }
                if (lines[count] == "Player 2:")
                {
                    addingToPlayerTwo = true;
                    count++;
                    continue;
                }
                if (addingToPlayerTwo)
                {
                    playerTwo.Enqueue(int.Parse(lines[count]));
                }
                else
                {
                    playerOne.Enqueue(int.Parse(lines[count]));
                }
                count++;
            }

            var originalHands = new List<Queue<int>>()
            {
                new Queue<int>(playerOne.ToArray()),
                new Queue<int>(playerTwo.ToArray())
            };

            //while (playerOne.Count > 0 && playerTwo.Count > 0)
            //{
            //    var p1Card = playerOne.Dequeue();
            //    var p2Card = playerTwo.Dequeue();
            //    if (p1Card > p2Card)
            //    {
            //        playerOne.Enqueue(p1Card);
            //        playerOne.Enqueue(p2Card);
            //    }
            //    else if (p1Card < p2Card)
            //    {
            //        playerTwo.Enqueue(p2Card);
            //        playerTwo.Enqueue(p1Card);
            //    }
            //}

            var winningPlayer = playerTwo.Count == 0 ? playerOne : playerTwo;

            long result = 0;
            int multiplyer = winningPlayer.Count;
            while (winningPlayer.Count > 0)
            {
                result += winningPlayer.Dequeue() * multiplyer;
                multiplyer--;
            }

            Console.WriteLine($"Solution 1: {result}");
            return originalHands;
        }

    }
}
