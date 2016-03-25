using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TelegramPersonalBot;

namespace TelegramPersonalBot.Parser.Expressions
{
    class Test : IExpression
    {
        #region Private properties
        private Context myContext;
        #endregion

        #region Interface methods
        public void SetParameters(List<string> e)
        {
            if (e != null && e.Count > 0)
                throw new ParseException("Test doesn't require any parameters.");
        }

        public void SetContext(Context c)
        {
            myContext = c;
        }

        public void Execute()
        {
            myContext.Message = "I'm glad to be able to let you know that this test is successfull.";
            myContext.Output = true;
        }

        public string HelpMessage()
        {
            return "Just a test expression to show you how it is done.";
        }
        #endregion
    }
}
