using System.Numerics;

namespace OOP_Lab5;

public static class InputHelper
{
	public static T? GetInput<T>(string name, Predicate<T> condition) where T : IParsable<T>
	{
		T? res;
		string? input;
		do
		{
			Console.Write($"Enter {name}: ");
			input = Console.ReadLine();
		} while (!T.TryParse(input, null, out res) || !condition(res));
		return res;
	}

	public static T? GetNonNegativeInput<T>(string name) where T : INumber<T>
		=> GetInput<T>(name, res => res >= T.Zero);

	public static string GetStringInput(string name, Predicate<string> condition)
	{
		string res;
		do
		{
			Console.Write($"Enter {name}: ");
			res = Console.ReadLine()!;
		} while (!condition(res));
		return res;
	}
}
