using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using TelegramPersonalBot.Parser;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TelegramPersonalBot
{
    class Program
    {
        #region Private properties
        private static bool stopMe = false;
        private static bool filterUsers = false;
        private static string accessToken;
        private static List<string> usernames = new List<string>();
        private static TelegramBot bot;
        #endregion

        #region Main
        public static void Main(string[] args)
        {
            HandleConfig();

            Console.WriteLine("Starting your bot...");
            Console.WriteLine();

            // Accepting all certificates
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

            // Start the Telegram bot
            var t = Task.Run(() => RunBot());

            // Handle ctrl-c
            Console.CancelKeyPress += delegate {
                stopMe = true;
            };

            if ((int) t.Status < 5)
                t.Wait();
        }
        #endregion

        #region Private methods
        public static void HandleConfig()
        {
            // Handle config
            accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (!bool.TryParse(ConfigurationManager.AppSettings["FilterUsers"], out filterUsers))
                filterUsers = false;

            if (filterUsers)
            {
                string un = ConfigurationManager.AppSettings["AllowedUsers"];
                if (!string.IsNullOrEmpty(un))
                {
                    usernames = un.Split(';').ToList();
                }
            }
        }

        private static void RunBot()
        {
            try
            {
                bot = new TelegramBot(accessToken);

                var me = bot.MakeRequestAsync(new GetMe()).Result;
                if (me == null)
                {
                    Console.WriteLine("GetMe() FAILED. Do you forget to add your AccessToken to the config?");
                    stopMe = true;
                    return;
                }
                Console.WriteLine("{0} (@{1}) connected!", me.FirstName, me.Username);

                Console.WriteLine();
                Console.WriteLine("Find @{0} in Telegram and send a message.", me.Username);
                Console.WriteLine("(Press ctrl-c to quit)");
                Console.WriteLine();

                long offset = 0;
                while (!stopMe)
                {
                    var updates = bot.MakeRequestAsync(new GetUpdates() { Offset = offset }).Result;
                    if (updates != null)
                    {
                        foreach (var update in updates)
                        {
                            offset = update.UpdateId + 1; // Update offset

                            // Authorize users
                            if (filterUsers && !IsUserAllowed(update.Message.From.Username))
                            {
                                SendReplyMessage("I'm sorry. You are not authorized.", update.Message);
                                continue;
                            }

                            // Ignore empty messaged
                            if (update.Message == null)
                            {
                                continue;
                            }


                            // Parse the command
                            try
                            {
                                Context c = new Context(update.Message.Text);
                                Parser.Parser p = new Parser.Parser(c);
                                IExpression exp = p.Parse();
                                if (exp == null)
                                {
                                    SendReplyMessage("I'm sorry. I do not understand '" + update.Message.Text + "'.", update.Message);
                                    continue;
                                }

                                exp.Execute();

                                if (c.Output)
                                {
                                    // return ok or the message
                                    if (string.IsNullOrEmpty(c.Message))
                                        SendReplyMessage("I've done your bidding, my master.", update.Message);
                                    else
                                        SendReplyMessage(c.Message, update.Message);
                                }
                            }
                            catch (ParseException)
                            {
                                SendReplyMessage("I'm sorry. I didn't understand what you are trying to tell me. Please try again or request '/help' or 'help'.", update.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: ", e.Message);
            }
        }

        private static bool IsUserAllowed(string username)
        {
            return usernames.Contains(username);
        }

        private static void SendReplyMessage(string message, Message original)
        {
            bot.MakeRequestAsync(new SendMessage(original.Chat.Id,
                message)
            {
                ParseMode = SendMessage.ParseModeEnum.Markdown
            }).Wait();

        }
        #endregion

        #region Certificate callback
        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
    }
}
