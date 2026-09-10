namespace Calculator;

public static class InfixToPostfixConverter
{
    private static readonly Dictionary<string, int> precedence = new Dictionary<string, int>
    {
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 },
        { "^", 3 },
        { "sqrt", 0 },
        { "%", 4 },
        { "(", 0 }
    };

    private static readonly Dictionary<string, bool> rightAssociative = new Dictionary<string, bool>
    {
        { "^", true }
    };

    public static List<string> InfixToPostfix(List<string> infixTokens)
    {
        var postfix = new List<string>();
        var operatorsStack = new Stack<string>();
        List<string> supportedFunctions = Enum.GetNames(typeof(Functions.SupportedFunctions)).ToList();

        foreach (var token in infixTokens)
        {
            if (double.TryParse(token, out _))
            {
                postfix.Add(token);
            }
            else if (supportedFunctions.Contains(token))
            {
                operatorsStack.Push(token);
            }
            else if (token == "(")
            {
                operatorsStack.Push(token);
            }
            else if (token == ")")
            {
                while (operatorsStack.Count > 0 && operatorsStack.Peek() != "(")
                {
                    postfix.Add(operatorsStack.Pop());
                }

                if (operatorsStack.Count == 0)
                    throw new FormatException("Invalid expression: unmatched ')'.");

                operatorsStack.Pop();
                if (operatorsStack.Count > 0 && supportedFunctions.Contains(operatorsStack.Peek()))
                    postfix.Add(operatorsStack.Pop());
            }
            else
            {
                while (operatorsStack.Count > 0 && precedence.ContainsKey(operatorsStack.Peek()) &&
                   ((rightAssociative.ContainsKey(token) && rightAssociative[token] && precedence[token] < precedence[operatorsStack.Peek()]) ||
                    (!rightAssociative.ContainsKey(token) && precedence[token] <= precedence[operatorsStack.Peek()])))
                {
                    postfix.Add(operatorsStack.Pop());
                }
                operatorsStack.Push(token);
            }
        }

        while (operatorsStack.Count > 0)
        {
            var op = operatorsStack.Pop();
            if (op == "(")
                throw new FormatException("Invalid expression: unmatched '('.");
            postfix.Add(op);
        }

        return postfix;
    }
}
