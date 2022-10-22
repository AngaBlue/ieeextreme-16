using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Language
{
    public class Node
    {
        public int NodeID = 0;

        public bool Leaf { get; set; } = false;
        public char Character { get; set; } = '\0';
        public string Language { get; set; } = string.Empty;

        public Node Present { get; set; }
        public int PresentID { get; set; }
        public Node NotPresent { get; set; }
        public int NotPresentID { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Construct the tree based on console input
            string input = Console.ReadLine();
            bool hasHeader = false;
            int nodeCount = 0;
            int phraseCount = 0;

            Node[] nodes = null;
            string[] phrases = null;
            int currentNodeIndex = 0;
            int currentPhraseIndex = 0;

            while (input != null && input.Length > 0)
            {
                // Only set hasHeader if it's not already true
                hasHeader = hasHeader || (input[0] == 'I' || input[0] == 'L');

                if (!hasHeader)
                {
                    // Starting line is the number of nodes and phrases
                    string[] splitHeader = input.Split(' ');
                    nodeCount = int.Parse(splitHeader[0]);
                    phraseCount = int.Parse(splitHeader[1]);

                    // Initialise node and phrase storage
                    nodes = new Node[nodeCount];
                    for (int i = 0; i < nodeCount; i++)
                    {
                        nodes[i] = new Node();
                    }

                    phrases = new string[phraseCount];
                }
                else
                {
                    // Crash if we're somehow getting here without a header
                    if (nodes == null || phrases == null)
                    {
                        throw new ArgumentNullException();
                    }

                    // Keep reading nodes until we reach the number that we already have
                    if (currentNodeIndex < nodeCount)
                    {
                        // Read a new node from the input
                        Node currentNode = nodes[currentNodeIndex++];

                        // Split the input on spaces
                        string[] splitNodeData = input.Split(' ');

                        // Parse the input
                        switch (input[0])
                        {
                            case 'I':
                                currentNode.Leaf = false;
                                break;
                            case 'L':
                                currentNode.Leaf = true;
                                break;
                            default:
                                currentNode.Leaf = false;
                                break;
                        }

                        // Node ID - nodes may be in arbitrary orders
                        currentNode.NodeID = int.Parse(splitNodeData[1]);

                        // Further data is dependent on whether it's a leaf or internal node
                        if (currentNode.Leaf)
                        {
                            // Last data is the language name
                            currentNode.Language = splitNodeData[2];
                        }
                        else
                        {
                            // 2 is the character
                            currentNode.Character = splitNodeData[2][0];

                            // 3 is the next node when present
                            currentNode.PresentID = int.Parse(splitNodeData[3]);

                            // and 4 is when it's not present
                            currentNode.NotPresentID = int.Parse(splitNodeData[4]);
                        }
                    }
                    else if (currentPhraseIndex < phraseCount)
                    {
                        // Store the current phrase
                        phrases[currentPhraseIndex++] = input;
                    }
                }

                input = Console.ReadLine();
            }

            // Create links based on the IDs
            foreach (Node node in nodes)
            {
                node.Present = nodes.Where(n => n.NodeID == node.PresentID).FirstOrDefault();
                node.NotPresent = nodes.Where(n => n.NodeID == node.NotPresentID).FirstOrDefault();
            }

            // We can now traverse the list - get the root node which is the one where none of them point to it
            // Get all of the indices that are pointed to
            HashSet<int> ChildIndices = new HashSet<int>();
            foreach (Node node in nodes)
            {
                ChildIndices.Add(node.PresentID);
                ChildIndices.Add(node.NotPresentID);
            }

            // Find the one node where it's not in the set
            Node rootNode = nodes.Where(n => !(ChildIndices.Contains(n.NodeID))).FirstOrDefault();

            // Check that we have a root
            if (rootNode == null)
            {
                throw new ArgumentNullException();
            }

            // Go through and traverse the phrases
            foreach (string phrase in phrases)
            {
                List<string> languages = new List<string>();

                // BFS node thing
                Queue<Node> nodeQueue = new Queue<Node>();
                nodeQueue.Enqueue(rootNode);

                while (nodeQueue.Count > 0)
                {
                    Node currentNode = nodeQueue.Dequeue();

                    // If it's a leaf node, then we definitely have a language
                    if (currentNode.Leaf)
                    {
                        languages.Add(currentNode.Language);
                    }
                    // Otherwise it's an internal node so we need to queue some stuff
                    else
                    {
                        // Check if it's present - if it's there, then it's definitely *not* one of the ones in the No tree
                        if (phrase.Contains(currentNode.Character))
                        {
                            nodeQueue.Enqueue(currentNode.Present);
                        }
                        // If it's not present, then it could be either one
                        // But it could always be one of the ones where it's used and it's just not available in this string
                        else
                        {
                            nodeQueue.Enqueue(currentNode.Present);
                            nodeQueue.Enqueue(currentNode.NotPresent);
                        }
                    }
                }

                // Sort the language list and spit it out
                languages.Sort();
                Console.WriteLine(string.Join(' ', languages));
            }
        }
    }
}