using Calculator;

// Calculator
// CLI
// Simple operations first: + -, *, /
// Obey order of operation: (), sqrt() exponent, * /, + -
// Use Double datatype
// Check that there are no double operators, with the exception of negative numbers.

bool quit = false;
double lastResult = 0;
int startParentheses = 0;
int endParentheses = 0;

Console.WriteLine("Calculator");
while (!quit)
{
    Console.WriteLine("Type 'q' and press 'enter' quit");
    //Console.WriteLine("Type 'c' and press 'enter' to clear the last result");
    Console.WriteLine("Type your expression below:");

    try
    {
        var input = Console.ReadLine();

        if (input.ToLower() == "q")
        {
            Console.WriteLine("Quiting Calculator");
            break;
        }

        //if (input.ToLower() == "c")
        //{
        //    lastResult = 0;
        //    continue;
        //}

        var cleanInput = input.Trim().Replace(" ", "").ToLower();

        startParentheses = cleanInput.Count('(');
        endParentheses = cleanInput.Count(')');

        if (startParentheses - endParentheses != 0)
            throw new FormatException("Invalid expression: Unbalanced amount of parentheses");

        double result = EvaluateExpression(cleanInput);
        Console.WriteLine($"Result : {result}");
        //lastResult = result;
    }
    catch (FormatException fe)
    {
        Console.WriteLine($"Invalid expression: {fe.Message}");
    }
    catch (DivideByZeroException dze)
    {
        Console.WriteLine($"Error : {dze.Message}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error : {e.Message}");
    }
}

static double EvaluateExpression(string expression)
{
    try
    {
        var tokens = Tokenizer.Tokenize(expression);
        var postfix = InfixToPostfixConverter.InfixToPostfix(tokens);
        return PostfixEvaluator.EvaluatePostfix(postfix);
    }
    catch (FormatException fe)
    {
        throw new FormatException(fe.Message);
    }
}
