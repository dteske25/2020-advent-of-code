using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdapterArray
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"R:\Code\2020-advent-of-code\10-adapter-array\data.txt";
            var lines = File.ReadAllLines(filePath).Select(c => int.Parse(c)).ToList();

            lines.Add(0);
            lines.Add(lines.Max() + 3);
            lines.Sort();

            PartOne(lines);
            PartTwo(lines);
        }

        static void PartOne(List<int> adapters)
        {
            var counts = new Dictionary<int, int>();
            for (int i = 0; i < adapters.Count - 1; i++)
            {
                var delta = adapters[i+1] - adapters[i];
                if (!counts.ContainsKey(delta))
                {
                    counts.Add(delta, 0);
                }
                counts[delta]++;
            }
            Console.WriteLine($"Solution 1: {counts[1] * counts[3]}");
        }

        static void PartTwo(List<int> adapters)
        {
            var lookups = new Dictionary<int, GraphNode<int>>();
            var graph = GetGraphDictionaryLookup(adapters, 0, adapters.Max(), lookups);
            var computedPaths = new Dictionary<int, long>();
            var count = GetGraphPathCountDFS(graph[0], adapters.Max(), computedPaths);

            Console.WriteLine($"Solution 2: {count}");
        }

        static Dictionary<int, GraphNode<int>> GetGraphDictionaryLookup(List<int> adapters, int currentValue, int max, Dictionary<int, GraphNode<int>> previousLookups)
        {
            if (currentValue == max)
            {
                var node = new GraphNode<int>() { Value = max };
                previousLookups.Add(currentValue, node);
            } 
            else if (!previousLookups.ContainsKey(currentValue))
            {
                var possibleList = adapters.Where(a => a > currentValue).ToList();

                var node = new GraphNode<int>()
                {
                    Value = currentValue,
                };
                foreach (var value in possibleList.Where(a => a <= max && a - currentValue <= 3))
                {
                    node.Neighbors.Add(GetGraphDictionaryLookup(possibleList, value, max, previousLookups)[value]);
                }
                previousLookups.Add(currentValue, node);
            }
            return previousLookups;
        }

        // Not efficient enough
        // static GraphNode<int> GetGraphNode(List<int> adapters, int currentValue, int max)
        // {
        //     if (currentValue == max)
        //     {
        //         return new GraphNode<int>() { Value = max };
        //     }
        //     // find neighbors

        //     var possibleList = adapters.Where(a => a > currentValue).ToList();

        //     return new GraphNode<int>()
        //     {
        //         Value = currentValue,
        //         Neighbors = possibleList.Where(a => a <= max && a - currentValue <= 3).Select(v => GetGraphNode(possibleList, v, max)).ToList()
        //     };
        // }

        static long GetGraphPathCountDFS(GraphNode<int> graph, int max, Dictionary<int, long> computedPaths)
        {
            if (graph.Value == max)
            {
                return 1;
            }
            if (computedPaths.ContainsKey(graph.Value))
            {
                return computedPaths[graph.Value];
            }

            var newlyComputed = graph.Neighbors.Sum(n => GetGraphPathCountDFS(n, max, computedPaths));
            computedPaths.Add(graph.Value, newlyComputed);
            return newlyComputed;
        }
    }

    public class GraphNode<TValue>
    {

        public TValue Value { get; set; }

        // get (and no set) - we don't want to assign the collection as whole
        // if we want to add/remove a neighbor we'll call Neighbors.Add, Neighbors.Remove
        public List<GraphNode<TValue>> Neighbors { get; set; } = new List<GraphNode<TValue>>();
    }
}
