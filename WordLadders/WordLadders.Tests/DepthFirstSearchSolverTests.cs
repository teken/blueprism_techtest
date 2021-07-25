using System;
using Xunit;

namespace WordLadders.Tests
{
    public class DepthFirstSearchSolverTests
    {
        [Theory]
        [InlineData(new string[]{"Spin","Spit","Spat","Spot","Span"}, "Spin", "Spot")]
        public void Solverable(string[] dictionary, string startWord, string endWord)
        {
            var solver = new DepthFirstSearchSolver(dictionary);
        
            var result = solver.Solve(startWord, endWord);
            Assert.IsNotNull(result, $"Solverable word ladder not solved, startword={startWord} endword={endWord}");
        }

        [Theory]
        [InlineData(new string[]{"Spin","Spat","Spot","Span"}, "Spin", "Spot")]
        public void Unsolverable(string[] dictionary, string startWord, string endWord)
        {
            var solver = new DepthFirstSearchSolver(dictionary);
        
            var result = solver.Solve(startWord, endWord);
            Assert.IsNull(result, $"Unsolverable word ladder solved, startword={startWord} endword={endWord}, ladder={result.Join(",")}");
        }
    }
}
