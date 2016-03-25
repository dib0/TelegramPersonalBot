using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelegramPersonalBot.Parser.Expressions
{
    class Help : IExpression
    {
        #region Private properties
        private Context myContext;
        private string expr = string.Empty;
        #endregion

        #region Interface methods
        public void SetParameters(List<string> e)
        {
            if (e != null && e.Count > 1)
                throw new ParseException("Help requires at most one parameter.");

            if (e.Count > 0)
            {
                expr = e[0];

                if (expr.StartsWith("/"))
                    expr = expr.Remove(0, 1);
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
                myContext.Message = "I'm glad to help you! Here is a list of commands. Use 'help <command>' for more detailed information.\n";

                List<Type> expressions = Parser.GetExpressions();
                foreach (Type t in expressions)
                    myContext.Message += t.Name + "\n";
            }
            else
            {
                IExpression e = Parser.FindExpression(expr);
                if (e != null)
                    myContext.Message = e.HelpMessage();
                else
                    myContext.Message = "I'm sorry. I do not know the expression " + expr;
            }
        }

        public string HelpMessage()
        {
            return "Meta help! :-$";
        }
        #endregion
    }
}
