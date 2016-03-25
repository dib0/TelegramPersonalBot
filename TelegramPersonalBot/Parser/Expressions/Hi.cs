using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TelegramPersonalBot.Parser.Expressions
{
    class Hi : IExpression
    {
        #region Private properties
        private Context myContext;
        #endregion

        #region Interface methods
        public void SetParameters(List<string> e)
        {
            if (e != null && e.Count > 0)
                throw new ParseException("Hi doesn't require any parameters.");
        }

        public void SetContext(Context c)
        {
            myContext = c;
        }

        public void Execute()
        {
            myContext.Message = "Hey you!";
            myContext.Output = true;
        }

        public string HelpMessage()
        {
            return "Says hi.";
        }
        #endregion
    }
}
