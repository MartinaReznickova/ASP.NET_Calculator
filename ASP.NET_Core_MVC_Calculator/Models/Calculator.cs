using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static Azure.Core.HttpHeader;

namespace ASP.NET_Core_MVC_Calculator.Models
{
    public class Calculator
    {

        [Required(ErrorMessage = "Do textového pole zadejte příklad k vypočítání.")]

        public string Input { get; set; } = "";
        public string InputOriginal { get; private set; } = "";
        private List<string> InputSplit { get; set; } = new List<string>();

        public double Result { get; private set; }

    
        public void GetResult()
        {

            InputOriginal = Input;
            //Input = Regex.Replace(Input, "^2", "^");
            Input = Regex.Replace(Input, "√", "s");

            AddSpaceAroundOperators();
            InputSplit = Input.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            Result = EvaluateRecursive(InputSplit);

        }

        public double EvaluateRecursive(List<string> expression)
        {

            if (expression.Contains("("))
            {
                int start = expression.LastIndexOf("(");
                int end = 0;
                int count = 0;

                for (int i = start; i < expression.Count; i++)
                {
                    if (expression[i] == ")")
                    {
                        end = i;
                        break;
                    }

                    count++;
                }

                List<string> innerExpression = expression.GetRange(start + 1, count - 1);
                double innerResult = EvaluateRecursive(innerExpression);

                expression.RemoveRange(start, count + 1);
                expression.Insert(start, innerResult.ToString());

                return EvaluateRecursive(expression);
            }

            if (expression.Contains("s")) {
                
                int index = expression.IndexOf("s");
                double result = Math.Sqrt(double.Parse(expression[index + 1]));

                expression[index] = result.ToString();
                expression.RemoveAt(index + 1);

                return EvaluateRecursive(expression);
            }

            if (expression.Contains("^"))
            {
                int index = expression.IndexOf("^");
                
                double value = double.Parse(expression[index - 1]);
                double power = double.Parse(expression[index + 1]);

                double result = Math.Pow(value, power);

                expression[index] = result.ToString();
                expression.RemoveAt(index - 1);
                expression.RemoveAt(index);   
             

                return EvaluateRecursive(expression);
            }


           

            if (expression.Contains("*"))
            {
                int index = expression.IndexOf("*");
                double firstNum;
                double secondNum;
                if (Double.TryParse(expression[index - 1], out firstNum) && Double.TryParse(expression[index + 1], out secondNum))
                {
                    double sum = firstNum * secondNum;
                    expression[index] = sum.ToString();
                    expression.RemoveAt(index - 1);
                    expression.RemoveAt(index);


                }

                return EvaluateRecursive(expression);
            }

        

            if (expression.Contains("/"))
            {
                int index = expression.IndexOf("/");
                double firstNum;
                double secondNum;
                if (Double.TryParse(expression[index - 1], out firstNum) && Double.TryParse(expression[index + 1], out secondNum))
                {
                    double sum = firstNum / secondNum;
                    expression[index] = sum.ToString();
                    expression.RemoveAt(index - 1);
                    expression.RemoveAt(index);
                   

                }

                return EvaluateRecursive(expression);

            }
            
            if (expression.Contains("+"))
            {
                int index = expression.IndexOf("+");
                double firstNum;
                double secondNum;
                if (Double.TryParse(expression[index - 1], out firstNum)&& Double.TryParse(expression[index + 1], out secondNum))
                {
                    double sum = firstNum + secondNum;
                    expression[index] = sum.ToString();
                    expression.RemoveAt(index - 1);
                    expression.RemoveAt(index);
                   

                }

                return EvaluateRecursive(expression);
            }

            if (expression.Contains("-"))
            {
                int index = expression.IndexOf("-");
                double firstNum;
                double secondNum;
                if (Double.TryParse(expression[index - 1], out firstNum) && Double.TryParse(expression[index + 1], out secondNum))
                {
                    double sum = firstNum - secondNum;
                    expression[index] = sum.ToString();
                    expression.RemoveAt(index - 1);
                    expression.RemoveAt(index);
                

                }

                return EvaluateRecursive(expression);
            }




           

            else 
            {
                double result = double.Parse(expression[0]);

                for(int i = 1; i< expression.Count; i++)
                {
                    result = double.Parse(expression[i]) * result;
                }

                return result;
                //return double.Parse(String.Join("", expression.ToArray())); 
            }
           
        }


        public void AddSpaceAroundOperators()
        {
            char[] operators = { '+', '-', '*', '/', ')', '(', '^', 's' };

            foreach (char op  in operators)
            {
                Input = Input.Replace(op.ToString(), $" {op} ");
            }

        }
        
        public bool AreTwoNumsNextToEachother()
        {  
                for (int i = 1; i < Input.Length - 1; i++)
                {
                if (Input[i] == ' ' && (char.IsDigit(Input[i - 1]) && char.IsDigit(Input[i + 1])))
                {
                    return true;
                }
            
            }
           

            return false;
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

        public bool IsInputEmpty()
        {
            if (string.IsNullOrEmpty(Input))
            {
                return true;
            }

            return false;
        }
    }
}
