using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramPersonalBot.Parser.Expressions
{
    class Start : IExpression
    {
        #region Private properties
        private Context myContext;
        #endregion

        #region Interface methods
        public void SetParameters(List<string> e)
        {
            if (e != null && e.Count > 0)
                throw new ParseException("Start doesn't require any parameters.");
        }

        public void SetContext(Context c)
        {
            myContext = c;
        }

        public void Execute()
        {
            string botname = ConfigurationManager.AppSettings["BotName"];
            if (string.IsNullOrEmpty(botname))
                botname = "Emily";

            myContext.Message = "Hi there! I'm " + botname + ", your friendly assistant. How can I help you?";
            myContext.Output = true;
        }

        public string HelpMessage()
        {
            return "Shows you a friendly welcoming message.";
        }
        #endregion
    }
}
