using System.Collections.Generic;

namespace WordLadders
{
    public interface IWordLadderSolver
    {
        public IEnumerable<string> Solve(string startWord, string endWord);
    }
}