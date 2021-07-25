using System;
using System.IO;
using Fclp;

namespace WordLadders
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
            var cliArgsParser = new FluentCommandLineParser<ApplicationArguments>();

            cliArgsParser.Setup(arg => arg.DictionaryFile).As('d', "DictionaryFile").Required();
            cliArgsParser.Setup(arg => arg.StartWord).As('s', "StartWord").Required();
            cliArgsParser.Setup(arg => arg.EndWord).As('e', "EndWord").Required();
            cliArgsParser.Setup(arg => arg.ResultFile).As('o', "ResultFile").Required(); //o for output
            cliArgsParser.SetupHelp("?", "help").Callback(text => Console.WriteLine(text));

            var result = cliArgsParser.Parse(args);

            if(result.HasErrors)
            {
                Console.Error.WriteLine($"Invalid arguments: {result.ErrorText}");
                return;
            }

            if (!File.Exists(cliArgsParser.Object.DictionaryFile)) {
                Console.Error.WriteLine($"Unable to find dictionary : {cliArgsParser.Object.DictionaryFile}");
                return;
            }
            string[] dictionary = File.ReadAllLines(cliArgsParser.Object.DictionaryFile);
            
            IWordLadderSolver solver = new DepthFirstSearchSolver(dictionary);

            var ladder = solver.Solve(cliArgsParser.Object.StartWord, cliArgsParser.Object.EndWord);
            if (ladder == null) {
                Console.Error.WriteLine("No valid ladder found");
                return;
            }

            } catch(Exception e) {
                Console.Error.WriteLine($"Application Failed: {e.Message}");
                return;
            }
        }
    }
}
