using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.Exceptions;
using ParserawkaCore.Utils;
using System;
using System.IO;

namespace ParserawkaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("# No arguments found");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine($"# Passed file not exists: {args[0]}");
                return;
            }
            string programCode;
            IProgramKnowledgeBase PKB;
            PKB = ImplementationFactory.CreateProgramKnowledgeBase();
            // TODO implementacja PQL-a
            try
            {
                programCode = File.ReadAllText(args[0]);
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"# Error during reading file: {args[0]}");
                return;
            }
            try
            {
                PKB.LoadData(programCode);
            }
            catch (LexerException e)
            {
                Console.Error.WriteLine($"# Error during parsing: {args[0]} in line: {e.Line}/ row: {e.Row} msg: {e.Message}");
                return;
            }
            catch (TreeBuildingException e)
            {
                Console.Error.WriteLine($"# Error during tree building: {args[0]} in line: {e.Line}/ row: {e.Row} msg: {e.Message}");
                return;
            }
            catch (ParserException e)
            {
                Console.Error.WriteLine($"# Error during processing: {args[0]} in line: {e.Line}/ row: {e.Row} msg: {e.Message}");
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"# Unhandled error during PKB building: {e.Message}");
                return;
            }
            Console.WriteLine("READY");
            while (true)
            {
                var declarations = Console.ReadLine();
                var query = Console.ReadLine();
                Console.WriteLine("TODO: RESULTS");
                // TODO zwracanie odpowiednich wyników w try-catch bloku z wypisaniem exceptionów
            }
        }
    }
}
