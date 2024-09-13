// See https://aka.ms/new-console-template for more information

using BLL;

Console.WriteLine("Hello, Dinosaur!"); 

var calc = new CalculatorBrain();

calc.Number1 = 1;
calc.Number2 = 2;

var res = calc.Add();
var res2 = calc.Subtract();

Console.WriteLine($"{calc.Number1} + {calc.Number2} = {res}");
Console.WriteLine($"{calc.Number1} - {calc.Number2} = {res2}");

