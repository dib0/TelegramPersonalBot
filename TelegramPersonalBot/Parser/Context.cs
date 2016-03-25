using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramPersonalBot.Parser
{
    class Context
    {
        #region Public properties
        public string Input
        {
            get; set;
        }

        public bool Output
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }
        #endregion

        #region Constructor
        public Context(string input)
        {
            Input = input;
        }
        #endregion
    }
}
