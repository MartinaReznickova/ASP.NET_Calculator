using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASP.NET_Core_MVC_Calculator.Models
{
    public class Calculator
    {

        [Required(ErrorMessage = "Do textového pole zadejte příklad k vypočítání.")]
        public string Input { get; set; } = "";

        public double Result { get; private set; }

       

        public void GetResult()
        {
            
            Input = Regex.Replace(Input, "sqrt", "s");

            Result = EvaluateRecursive(Input);

        }

        public double EvaluateRecursive(string expression)
        {
            if (expression.Contains("+"))
            {

            }
            
            if (expression.Contains ("("))
            {
                int start = expression.LastIndexOf ("(");
                int end = 0;

                for (int i = start; i < expression.Length; i++)
                {
                    if (expression[i] == ')') {
                        end = i; 
                        break;
                    }
                }

                string innerExpression = expression.Substring(start + 1, end - start - 1);

                double innerResult = EvaluateRecursive(innerExpression);

                expression = expression.Substring(0, start) + innerResult + expression.Substring(end + 1);

                return EvaluateRecursive(expression);
            }

            if (expression.Contains ("s(")) { 

                int start = expression.IndexOf ("s(");
                int end = 0;

                for (int i = start; i < expression.Length; i++)
                {
                    if (expression[i] == ')')
                    {
                        end = i;
                        break;
                    }
                }

                string innerExpression = expression.Substring(start + 1, end - start - 2);

                double innerResult = EvaluateRecursive(innerExpression);

                expression = expression.Substring(0, start) + Math.Sqrt(innerResult) + expression.Substring(end + 1);

                return EvaluateRecursive(expression);


            }

            if (expression.Contains("^"))
            {
                string[] parts = expression.Split('^');
                double value = double.Parse(parts[0].Trim());
                double power = double.Parse(parts[1].Trim());

                return Math.Pow(value, power);
            }
           

            else
            {
                return double.Parse(expression);
            }
        }


        public bool IsNumOfBracketsEven()
        {
            Input = Regex.Replace(Input, "{", "(");
            Input = Regex.Replace(Input, "}", ")");
            Input = Regex.Replace(Input, "\\[", ")");
            Input = Regex.Replace(Input, "]", ")");
        
            
            int leftBrackets = 0;
            int rightBrackets = 0;

            foreach (var c in Input)
            {
                if (c == '(')
                {
                    leftBrackets++;
                }

                else if (c == ')')
                {
                    rightBrackets++;
                }
            }

            return leftBrackets == rightBrackets;
        }

        public bool IsOperatorDuplicate()
        {
            char[] operators = { '+', '-', '*', '/', '.' };

           for(int i = 0; i < Input.Length - 1; i++) 
            { 
                if (operators.Contains(Input[i]) && operators.Contains(Input[i+1])) 
                    {
                        return true;
                    }
            }

           return false;

        }

        public bool IsDotBetweenNums()
        {
            for (int i = 1; i < Input.Length - 1; i++)
            {
                if (Input[i] == '.' && ( !char.IsDigit(Input[i-1]) || !char.IsDigit(Input[i + 1]))){
                    return false;
                }
            }

            return true;

        }

        public bool IsUnallowedOperatorAtBeginOrEnd()
        {
            char[] unallowedOperatorsStart = { '.', '/', '*' };
            char[] unallowedOperatorsEnd = { '-', '+','.', '/', '*', '√' };

            if (unallowedOperatorsStart.Contains(Input[0]) || unallowedOperatorsEnd.Contains(Input[Input.Length - 1]))
            {
                return true;
            }

            else return false;
        }

        public bool IsLetterInInput() => Input.Any(x => char.IsLetter(x));
    }
}
