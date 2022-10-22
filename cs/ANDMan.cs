using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ANDMan
{
    public class Node
    {
        public int NodeID;
        public Node Parent { get; set; }

        public List<Node> Children = new List<Node>();
        public long Weight { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int lineCount = 0;
            int testCaseCount = 0;

            // First line is the number of test cases
            testCaseCount = int.Parse(Console.ReadLine());

            string input = Console.ReadLine();

            int nodeCount = 0;
            Node[] nodes = new Node[0];

            bool newTestCase = true;
            int currTcLineNumber = 0;
            int remainingLines = 1;
            int operationCount = 0;

            // Keep getting input until there's nothing left
            while (input != null && input.Length > 0)
            {
                // Check if we've reached the end of the test case
                if (remainingLines <= 0)
                {
                    // Reset everything
                    currTcLineNumber = 0;
                    remainingLines = 1;
                    operationCount = 0;

                    nodeCount = 0;
                    nodes = new Node[0];
                }

                // New test case, so read the number of weights
                if (currTcLineNumber == 0)
                {
                    nodeCount = int.Parse(input);
                    remainingLines += (nodeCount + 1);
                    // Initialise all of the nodes
                    nodes = new Node[nodeCount];
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodes[i] = new Node();
                        nodes[i].NodeID = i;
                    }
                }
                // Second line is the weights
                else if (currTcLineNumber == 1)
                {
                    long[] weights = input.Split(' ').Select(w => long.Parse(w)).ToArray();
                    
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodes[i].Weight = weights[i];
                    }
                }
                // Until n is all of the edges
                else if (currTcLineNumber <= nodeCount)
                {
                    // Format here is [u] [v]
                    int[] nodeIndices = input.Split(' ').Select(i => int.Parse(i)).ToArray();

                    // u is the parent node, v is the child
                    nodes[nodeIndices[0] - 1].Children.Add(nodes[nodeIndices[1] - 1]);
                    // Since it's a tree, we're guaranteed to only have a single parent
                    nodes[nodeIndices[1] - 1].Parent = nodes[nodeIndices[0] - 1];
                }
                // This is the number of operations we're expecting for this test case - Q in the statement
                else if (currTcLineNumber == nodeCount + 1)
                {
                    operationCount = int.Parse(input);
                    remainingLines += operationCount;
                }
                // Parse and perform all of the operations
                else
                {
                    // Operation type is dependent on the first character in the string
                    if (input[0] == '1')
                    {
                        // Input format is [TYPE] [ID] [WEIGHT]
                        long[] data = input.Split(' ').Select(i => long.Parse(i)).ToArray();
                        nodes[data[1] - 1].Weight = data[2];
                    }
                    else if (input[0] == '2')
                    {
                        int[] data = input.Split(' ').Select(i => int.Parse(i)).ToArray();
                        Node target1 = nodes[data[1] - 1];
                        Node target2 = nodes[data[2] - 1];

                        // Construct a list of the paths back to the root node
                        List<Node> target1ToRoot = new List<Node>();
                        target1ToRoot.Add(target1);

                        Node p1 = target1;
                        while (p1 != null)
                        {
                            target1ToRoot.Add(p1.Parent);
                            p1 = p1.Parent;
                        }

                        List<Node> target2ToRoot = new List<Node>();
                        target2ToRoot.Add(target2);

                        Node p2 = target2;
                        while (p2 != null)
                        {
                            target2ToRoot.Add(p2.Parent);
                            p2 = p2.Parent;
                        }

                        // Figure out where they diverge - remove the first one because that'll be null
                        target1ToRoot.RemoveAll(n => n == null);
                        target2ToRoot.RemoveAll(n => n == null);
                        target1ToRoot.Reverse();
                        target2ToRoot.Reverse();

                        int lastCommonAncestorIndex = 0;

                        for (int i = 0; i < Math.Min(target1ToRoot.Count, target2ToRoot.Count); i++)
                        {
                            // If the item is different, then we've split paths
                            if (target1ToRoot[i] != target2ToRoot[i])
                            {
                                lastCommonAncestorIndex = i - 1;
                                break;
                            }
                            else if (i ==  (Math.Min(target1ToRoot.Count, target2ToRoot.Count) - 1))
                            {
                                lastCommonAncestorIndex = i;
                                break;
                            }
                        }

                        // Construct a path from the target back to the ancestor, then to target 2
                        List<Node> path = new List<Node>();
                        for (int i = target1ToRoot.Count - 1; i >= lastCommonAncestorIndex; i--)
                        {
                            path.Add(target1ToRoot[i]);
                        }
                        for (int i = lastCommonAncestorIndex + 1; i < target2ToRoot.Count; i++)
                        {
                            path.Add(target2ToRoot[i]);
                        }

                        // Traverse the path, multiply by the weight, and print the number
                        long currentWeight = 1;
                        foreach (Node node in path)
                        {
                            currentWeight *= node.Weight;
                        }

                        Console.WriteLine(currentWeight % 1000000007L);
                    }
                }

                // Decrement the number of expected remaining lines
                remainingLines--;
                currTcLineNumber++;

                input = Console.ReadLine();
                lineCount++;
            }
        }
    }
}