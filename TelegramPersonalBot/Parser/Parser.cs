using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace TelegramPersonalBot.Parser
{
    internal class Parser
    {
        #region Private properties
        private Context myContext;

        private static List<Type> expressionTypes = new List<Type>();
        #endregion

        #region Constructor
        public Parser(Context context)
        {
            myContext = context;
        }
        #endregion

        #region Public methods
        public IExpression Parse()
        {
            List<string> expr = myContext.Input.Split(' ').ToList();

            if (expr[0].StartsWith("/"))
                expr[0] = expr[0].Remove(0, 1);
            if (expr[expr.Count - 1].EndsWith("!"))
                expr[expr.Count - 1] = expr[expr.Count - 1].Remove(expr[expr.Count - 1].Length -1 , 1);
            if (expr[expr.Count - 1].EndsWith("?"))
                expr[expr.Count - 1] = expr[expr.Count - 1].Remove(expr[expr.Count - 1].Length - 1, 1);
            if (expr[expr.Count - 1].EndsWith("."))
                expr[expr.Count - 1] = expr[expr.Count - 1].Remove(expr[expr.Count - 1].Length - 1, 1);

            IExpression e = Parser.FindExpression(expr[0]);
            expr.RemoveAt(0);

            if (e != null)
            {
                e.SetParameters(expr);
                e.SetContext(myContext);
            }

            return e;
        }
        #endregion

        #region Private methods
        internal static List<Type> GetExpressions()
        {
            if (expressionTypes.Count() == 0)
            {
                List<Type> classes = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "TelegramPersonalBot.Parser.Expressions", StringComparison.Ordinal)).ToList();
                SelectExpressionsFromTypes(classes, expressionTypes);
                AddExternalExpressions(expressionTypes);
            }

            return expressionTypes;
        }

        private static void AddExternalExpressions(List<Type> result)
        {
            string assemblyName = ConfigurationManager.AppSettings["ExpressionAssembly"];

            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly asm = Assembly.Load(assemblyName);

                List<Type> classes = asm.GetTypes().ToList();
                SelectExpressionsFromTypes(classes, result);
            }
        }

        private static void SelectExpressionsFromTypes(List<Type> classes, List<Type> result)
        {
            if (classes == null || classes.Count() == 0)
                return;

            foreach (Type t in classes)
            {
                List<Type> interfaces = t.GetInterfaces().ToList();
                Type inType = interfaces.FirstOrDefault(i => String.Equals(i.Name, "IExpression", StringComparison.Ordinal));

                if (inType != null)
                    result.Add(t);
            }
        }

        internal static IExpression FindExpression(string name)
        {
            IExpression result = null;

            List<Type> ins = GetExpressions();
            Type e = ins.FirstOrDefault(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));

            if (e != null)
            {
                result = (IExpression) Activator.CreateInstance(e);
            }

            return result;
        }
        #endregion
    }
}
