using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramPersonalBot.Parser.Expressions
{
    class Please : IExpression
    {
        #region Private properties
        private Context myContext;
        private string expr = string.Empty;
        #endregion

        #region Interface methods
        public void SetParameters(List<string> e)
        {
            if (e != null && e.Count > 0)
            {
                foreach (string s in e)
                {
                    if (expr.Length > 0)
                        expr += " ";

                    expr += s;
                }
            }
        }

        public void SetContext(Context c)
        {
            myContext = c;
        }

        public void Execute()
        {
            myContext.Output = true;
            if (string.IsNullOrEmpty(expr))
            {
                myContext.Message = "Yes, my master?";
                return;
            }

            Context c = new Context(expr);
            Parser p = new Parser(c);
            IExpression exp = p.Parse();
            if (exp == null)
            {
                myContext.Message = "I'm sorry. I do not understand '" + expr + "'.";
            }

            exp.Execute();

            if (c.Output)
                myContext.Message = c.Message;
            else
                myContext.Message = "I'm sorry. I've failed you.";
        }

        public string HelpMessage()
        {
            return "Please doesn't do anything, but make me more happy to do your bidding.";
        }
        #endregion
    }
}
