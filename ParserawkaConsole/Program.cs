using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.Exceptions;
using ParserawkaCore.PQL;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Model;
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
                var sqlStatement = declarations + " " + query;
                try
                {
                    PqlLexer lexer = new PqlLexer(sqlStatement);
                    PqlParser parser = new PqlParser(lexer);
                    PqlAst pqlAst = parser.Parse();
                    PqlEvaluator pqlEvaluator = new PqlEvaluator(PKB, pqlAst);
                    PqlOutput pqlOutput = pqlEvaluator.Evaluate();
                    Console.WriteLine(pqlOutput.ToString());
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"#{e.Message}");
                }
            }
        }
    }
}
