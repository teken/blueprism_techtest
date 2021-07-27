using System.Collections.Generic;
using System.Linq;

namespace WordLadders
{
    public class BreadthFirstSearchSolver : IWordLadderSolver
    {
        public IEnumerable<string> Dictionary { get; }

        public BreadthFirstSearchSolver(string[] dictionary)
        {
            Dictionary = new List<string>(dictionary).Select(x => x.ToLower());
        }

        public IEnumerable<string> Solve(string startWord, string endWord)
        {
            startWord = startWord.ToLower();
            endWord = endWord.ToLower();

            if (!Dictionary.Contains(startWord)) return null;
            if (!Dictionary.Contains(endWord)) return null;

            var containedLetters = startWord.ToCharArray().Concat(endWord.ToCharArray()).ToHashSet();

            var filteredDictionary = Dictionary.Where(x => x.Length == startWord.Length && containedLetters.Any(y => x.Contains(y)));

            var graph = new Graph(filteredDictionary.ToList());

            var (found, path) = graph.BFSearch(startWord, endWord);
            return found ? path : null;
        }
    }
}