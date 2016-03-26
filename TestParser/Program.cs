using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramPersonalBot.Parser;

namespace TestParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser("/start");
            Parser("Hi!");
            Parser("Help");
            Parser("Help help");
            Parser("Please help \"help me\"");
        }

        static void Parser(string input)
        {
            Console.WriteLine("Input: {0}", input);

            Context c = new Context(input);
            Parser p = new Parser(c);
            IExpression exp = p.Parse();

            exp.Execute();
            if (c.Output)
                Console.WriteLine("Parse: ok!");
            else
                Console.WriteLine("Parse: not ok!");

            Console.WriteLine("Output: {0}", c.Message);
            Console.WriteLine();
        }
    }
}
