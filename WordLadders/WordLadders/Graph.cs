using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLadders
{
    public class Graph {
        private List<GraphNode> nodes;

        public Graph(List<string> dictionary)
        {
            nodes = dictionary.Select(x => new GraphNode(x.ToLower())).ToList();
            nodes.ForEach(x => x.FindLinks(nodes));
        }

        public (bool, IEnumerable<string>) BFSearch(string startWord, string endWord)
        {
            var startNode = nodes.First(x => x.Word == startWord.ToLower());
            var endNode = nodes.First(x => x.Word == endWord.ToLower());

            HashSet<string> visited = new HashSet<string>();
            Queue<GraphNode> queue = new Queue<GraphNode>();
            queue.Enqueue(startNode);


            bool found = false;
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Word == endNode.Word)
                {
                    found = true;
                    break;
                }
                if (visited.Contains(node.Word))
                    continue;
                else
                    visited.Add(node.Word);

                if (node.Edges == null || node.Edges.Count == 0) continue;

                node.Edges.Where(x => !visited.Contains(x.Word)).ToList().ForEach(x =>
                {
                    x.Parent = node;
                    queue.Enqueue(x);
                });
            }
            if (!found)
                return (false, null);

            var ladder = new List<string>();
            var currentNode = endNode;
            while (currentNode.Word != startNode.Word)
            {
                ladder.Add(currentNode.Word);
                currentNode = currentNode.Parent;    
            }
            ladder.Add(startNode.Word);

            ladder.Reverse();
            return (true, ladder);
        }

        [Obsolete("has serious proformance issues at scale")]
        public (bool, IEnumerable<string>) DFSearch(string startWord, string endWord)
        {
            var startNode = nodes.First(x => x.Word == startWord.ToLower());
            var endNode = nodes.First(x => x.Word == endWord.ToLower());
            bool found = false;
            List<GraphNode> path = new List<GraphNode>();

            Dictionary<GraphNode, bool> visited = nodes.ToDictionary(x => x, x => false);
            visited[startNode] = true;

            var currentNode = startNode;
            while(!found)
            {
                visited[currentNode] = true;
                found = currentNode.Edges.Any(x => x == endNode);
                path.Add(currentNode);

                if (!found)
                {
                    if (currentNode.Edges.Any(x => !visited[x]))
                        currentNode = currentNode.Edges.First(x => !visited[x]);
                    else
                        return (false, null);
                }
            }

            path.Add(endNode);
            return (found, path.Select(x => x.Word));
        }
    }

    class GraphNode : IEquatable<GraphNode>
    {
        public string Word { get; }

        public List<GraphNode> Edges { get; set; }

        public GraphNode Parent { get; set; }

        public GraphNode(string nodeValue)
        {
            Word = nodeValue;
        }

        public void FindLinks(IEnumerable<GraphNode> nodes)
        {
            Edges = nodes.Where(x => WordsDistance(Word, x.Word) == 1).ToList();
        }

        private int WordsDistance(string wordA, string wordB)
        {
            if (wordA == wordB) return 0;
            var wordALength = wordA.Length;
            var wordBLength = wordB.Length;
            //if (wordALength != wordBLength && (wordALength - 1) != wordBLength && (wordALength + 1) != wordBLength) return false;

            int distance = 0;

            if (wordALength > wordBLength) for (int i = 0; i < wordALength - wordBLength; i++)
            {
                wordB += " ";
            }

            for (int i = 0; i < wordALength; i++) if (wordA[i] != wordB[i]) distance++;

            distance += wordBLength - wordALength;

            return distance;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GraphNode);
        }

        public bool Equals(GraphNode other)
        {
            return other != null &&
                   Word == other.Word;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Word);
        }

        public static bool operator ==(GraphNode a, GraphNode b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return a.Word == b.Word;
        }

        public static bool operator !=(GraphNode a, GraphNode b)
        {
            if (a == null && b == null) return false;
            if (a == null || b == null) return true;
            return a.Word != b.Word;
        }
    }
}
