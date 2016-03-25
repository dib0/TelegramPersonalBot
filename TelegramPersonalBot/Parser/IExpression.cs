using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramPersonalBot.Parser
{
    public interface IExpression
    {
        void SetContext(Context c);
        void SetParameters(List<string> e);
        void Execute();
        string HelpMessage();
    }
}
