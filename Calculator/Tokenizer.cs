using System.Text;

namespace Calculator;

public static class Tokenizer
{
    public static List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        var number = new StringBuilder();
        var function = new StringBuilder();
        List<string> supportedFunctions = Enum.GetNames(typeof(Functions.SupportedFunctions)).ToList();

        bool lastWasOperator = true;
        bool parentheseExpected = false;

        foreach (char ch in expression)
        {
            if (parentheseExpected == true && !"(".Contains(ch))
            {
                throw new FormatException($"Invalid expression: '(' expected after function call");
            }

            if (char.IsDigit(ch) || ch == '.')
            {
                number.Append(ch);
                lastWasOperator = false;
            }
            else if (char.IsLetter(ch))
            {
                function.Append(ch);
                lastWasOperator = false;
            }
            else if ("+-*/%^()".Contains(ch))
            {
                if (number.Length > 0)
                {
                    tokens.Add(number.ToString());
                    number.Clear();
                }

                if (function.Length > 0)
                {
                    if (supportedFunctions.Contains(function.ToString()))
                    {
                        tokens.Add(function.ToString());
                        function.Clear();
                        parentheseExpected = true;
                    }
                    else
                        throw new FormatException($"Invalid operator: {function.ToString()}. Supported operators: +, -, *, /, ^, %, sqrt");
                }

                if ((ch == '-' || ch == '+') && lastWasOperator)
                {
                    number.Append(ch);
                }
                else
                {
                    tokens.Add(ch.ToString());
                    lastWasOperator = ch != ')';
                    parentheseExpected = false;
                }
            }
            else if (!char.IsWhiteSpace(ch))
            {
                throw new FormatException($"Invalid operator: {ch}. Supported operators: +, -, *, /, ^, %, sqrt");
            }
        }

        if (number.Length > 0)
            tokens.Add(number.ToString());

        if (lastWasOperator)
            throw new FormatException("Invalid expression: cannot end on an operator.");

        if (function.Length > 0)
            throw new FormatException("Invalid expression: '(' expected after function call");

        return tokens;
    }
}
