using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConwayCubes
{
    static class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\ConwayCubes\data.txt";
            var lines = File.ReadAllLines(filePath).ToList();

            PartOne(lines);
            PartTwo(lines);
        }

        private static List<string> GetNeighborKeysTwo(int x, int y, int z, int w)
        {
            var neighborKeys = new List<string>();
            var key = $"{x},{y},{z},{w}";
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        for (int l = w - 1; l <= w + 1; l++)
                        {
                            var tempKey = $"{i},{j},{k},{l}";
                            if (tempKey != key)
                            {
                                neighborKeys.Add(tempKey);
                            }
                        }
                            
                    }
                }
            }
            return neighborKeys;
        }

        private static bool ShouldBecomeActiveTwo(Dictionary<string, bool> state, int x, int y, int z, int w)
        {
            int activeCount = 0;
            var key = $"{x},{y},{z},{w}";
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        for (int l = w - 1; l <= w + 1; l++)
                        {
                            var tempKey = $"{i},{j},{k},{l}";
                            if (tempKey != key && state.ContainsKey(tempKey) && state[tempKey])
                            {
                                activeCount++;
                            }
                        }
                    }
                }
            }
            if (state[key])
            {
                return activeCount == 2 || activeCount == 3;
            }
            return activeCount == 3;
        }

        private static void PartTwo(List<string> lines)
        {
            var temp = new Dictionary<string, bool>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    temp.Add($"{i},{j},0,0", lines[i][j] == '#');
                }
            }
            var state = new Dictionary<string, bool>();
            foreach (var kvp in temp)
            {
                var indexes = kvp.Key.Split(',').Select(token => int.Parse(token)).ToList();
                var keysToAdd = GetNeighborKeysTwo(indexes[0], indexes[1], indexes[2], indexes[3]);
                state.AddOrUpdate(kvp.Key, kvp.Value);
                foreach (var keyToAdd in keysToAdd)
                {
                    if (!state.ContainsKey(keyToAdd))
                    {
                        state.Add(keyToAdd, false);
                    }
                }
            }

            for (var iteration = 0; iteration < 6; iteration++)
            {
                var tempState = new Dictionary<string, bool>();
                foreach (var kvp in state)
                {
                    var indexes = kvp.Key.Split(',').Select(token => int.Parse(token)).ToList();
                    var value = ShouldBecomeActiveTwo(state, indexes[0], indexes[1], indexes[2], indexes[3]);
                    tempState.AddOrUpdate(kvp.Key, value);
                    var keysToAdd = GetNeighborKeysTwo(indexes[0], indexes[1], indexes[2], indexes[3]);
                    foreach (var keyToAdd in keysToAdd)
                    {
                        if (!tempState.ContainsKey(keyToAdd))
                        {
                            tempState.Add(keyToAdd, false);
                        }
                    }
                }
                state = tempState;
            }
            var result = state.Values.Where(v => v).Count();
            Console.WriteLine($"Solution 2: {result}");
        }

        private static List<string> GetNeighborKeys(int x, int y, int z)
        {
            var neighborKeys = new List<string>();
            var key = $"{x},{y},{z}";
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        var tempKey = $"{i},{j},{k}";
                        if (tempKey != key)
                        {
                            neighborKeys.Add(tempKey);
                        }
                    }
                }
            }
            return neighborKeys;
        }

        private static bool ShouldBecomeActive(Dictionary<string, bool> state, int x, int y, int z)
        {
            int activeCount = 0;
            var key = $"{x},{y},{z}";
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        var tempKey = $"{i},{j},{k}";
                        if (tempKey != key && state.ContainsKey(tempKey) && state[tempKey])
                        {
                            activeCount++;
                        }
                    }
                }
            }
            if (state[key])
            {
                return activeCount == 2 || activeCount == 3;
            }
            return activeCount == 3;
        }

        private static void PartOne(List<string> lines)
        {
            var temp = new Dictionary<string, bool>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    temp.Add($"{i},{j},0", lines[i][j] == '#');
                }
            }
            var state = new Dictionary<string, bool>();
            foreach (var kvp in temp)
            {
                var indexes = kvp.Key.Split(',').Select(token => int.Parse(token)).ToList();
                var keysToAdd = GetNeighborKeys(indexes[0], indexes[1], indexes[2]);
                state.AddOrUpdate(kvp.Key, kvp.Value);
                foreach (var keyToAdd in keysToAdd)
                {
                    if (!state.ContainsKey(keyToAdd))
                    {
                        state.Add(keyToAdd, false);
                    }
                }
            }

            for (var iteration = 0; iteration < 6; iteration++)
            {
                var tempState = new Dictionary<string, bool>();
                foreach (var kvp in state)
                {
                    var indexes = kvp.Key.Split(',').Select(token => int.Parse(token)).ToList();
                    var value = ShouldBecomeActive(state, indexes[0], indexes[1], indexes[2]);
                    tempState.AddOrUpdate(kvp.Key, value);
                    var keysToAdd = GetNeighborKeys(indexes[0], indexes[1], indexes[2]);
                    foreach (var keyToAdd in keysToAdd)
                    {
                        if (!tempState.ContainsKey(keyToAdd))
                        {
                            tempState.Add(keyToAdd, false);
                        }
                    }
                }
                state = tempState;
            }
            var result = state.Values.Where(v => v).Count();
            Console.WriteLine($"Solution 1: {result}");
        }

        public static void AddOrUpdate<T, V>(this Dictionary<T, V> dict, T key, V value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}
