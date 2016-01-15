using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace TestConsole
{
    internal class Program
    {
        #region Implementation
        private static void DoMainJob(string input)
        {
            Console.WriteLine(input + "\n");

            var pattern = InputPattern();
            var delegateBody = InputDelegateBody();
            var matchEvaluator = DynamicDelegateGeneration<MatchEvaluator>.CreateDelegateFromBody(
                delegateBody);

            var result = Regex.Replace(input, pattern, matchEvaluator);
            Console.WriteLine("\nResult:\n{0}\n", result);
            Console.ReadLine();
        }

        private static string GetInputText()
        {
            const string FILE = "test.txt";
            var input = File.ReadAllText(FILE);
            return input;
        }

        private static string InputDelegateBody()
        {
            Console.WriteLine("Input match evaluate:");
            var lines = new List<string>();

            string line;
            do
            {
                line = Console.ReadLine();
                lines.Add(line);
            } while (line == null || !line.StartsWith("return"));

            var delegateBody = string.Join(Environment.NewLine, lines);
            return delegateBody;
        }

        private static string InputPattern()
        {
            Console.Write("Input pattern: ");
            return Console.ReadLine();
        }

        private static void Main()
        {
            var input = GetInputText();

            while (true)
            {
                try
                {
                    DoMainJob(input);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception + "\n");
                }
            }

            // ReSharper disable once FunctionNeverReturns
        }
        #endregion
    }
}