namespace WordLadders
{
    public class DepthFirstSearchSolver : IWordLadderSolver
    {
        public string[] Dictionary { get; }
        public DepthFirstSearchSolver(string[] dictionary)
        {
            this.Dictionary = dictionary;

        }

        public string[] Solve(string startWord, string endWord)
        {
            throw new System.NotImplementedException();
        }
    }
}