namespace Calculator;

public static class PostfixEvaluator
{
    public static double EvaluatePostfix(List<string> postfixTokens)
    {
        var stack = new Stack<double>();
        List<string> supportedFunctions = Enum.GetNames(typeof(Functions.SupportedFunctions)).ToList();

        foreach (var token in postfixTokens)
        {
            if (double.TryParse(token, out var number))
            {
                stack.Push(number);
            }
            else
            {
                if (supportedFunctions.Contains(token))
                {
                    if (stack.Count < 1) throw new FormatException("Invalid expression. Please check the input.");

                    var x = stack.Pop();

                    switch (token)
                    {
                        case "sqrt":
                            stack.Push(Calculator.Sqrt(x));
                            break;
                        default:
                            throw new FormatException($"Invalid function: {token}. Supported operators: sqrt");
                    }
                }
                else
                {
                    if (stack.Count < 2) throw new FormatException("Invalid expression. Please check the input.");

                    var num2 = stack.Pop();
                    var num1 = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(Calculator.Add(num1, num2));
                            break;
                        case "-":
                            stack.Push(Calculator.Subtract(num1, num2));
                            break;
                        case "*":
                            stack.Push(Calculator.Multiply(num1, num2));
                            break;
                        case "/":
                            stack.Push(Calculator.Divide(num1, num2));
                            break;
                        case "^":
                            stack.Push(Calculator.Power(num1, num2));
                            break;
                        case "%":
                            stack.Push(Calculator.Percentage(num1, num2));
                            break;
                        default:
                            throw new FormatException($"Invalid operator: {token}. Supported operators: +, -, *, /, ^, %");

                    }
                }
            }
        }

        if (stack.Count != 1)
        {
            throw new FormatException("Invalid expression. Please check the input.");
        }

        return stack.Pop();
    }
}
