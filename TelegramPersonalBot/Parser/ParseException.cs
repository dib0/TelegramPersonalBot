using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelegramPersonalBot.Parser
{
    [System.Serializable]
    public class ParseException : Exception
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
        protected ParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
