using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;

namespace OOP_Lab5;

public class Program
{
	static T? GetInput<T>(string name, Predicate<T> condition) where T : IParsable<T>
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

	static T? GetNonNegativeInput<T>(string name) where T : INumber<T>
		=> GetInput<T>(name, res => res >= T.Zero);

	static string GetStringInput(string name, Predicate<string> condition)
	{
		string res;
		do
		{
			Console.Write($"Enter {name}: ");
			res = Console.ReadLine()!;
		} while (!condition(res));
		return res;
	}

	const string DataDirectory = @"D:\Sources\University\2 course\OOP\OOP-Lab5\Data";

	static readonly Dictionary<string, IEnumerable<Tax>> dataCollection = new();

	static readonly Dictionary<char, (Action Action, string Description)> actions = new()
	{
		['l'] = (Look, "look through existing data"),
		['c'] = (Create, "create data"),
		['f'] = (Files, "look through files in data directory"),
		['w'] = (Write, "write data to file"),
		['r'] = (Read, "read data from file")
	};

	static void Look()
	{
		if (dataCollection.Count == 0)
		{
			Console.WriteLine("Data collection is empty");
			return;
		}

		foreach (var (name, taxes) in dataCollection)
		{
			Console.WriteLine($"{name}:");
			Console.WriteLine(string.Join(Environment.NewLine, taxes.Select(t => $"-> {t}")));
		}
	}

	static void Create()
	{
		string name = GetStringInput("name", s => !string.IsNullOrEmpty(s));

		var data = new TaxpayerData(
			MainJobIncome: GetNonNegativeInput<double>("main job income"),
			AdditionalJobIncome: GetNonNegativeInput<double>("additional job income"),
			AuthorAwardsAmount: GetNonNegativeInput<double>("author awards amount"),
			GoodsSellsIncome: GetNonNegativeInput<double>("goods sells income"),
			MoneyGiftsAmount: GetNonNegativeInput<double>("money gifts amount"),
			GoodsGiftsAmount: GetNonNegativeInput<double>("goods gifts amount"),
			AbroadTransfersAmount: GetNonNegativeInput<double>("abroad transfers amount"),
			ChildrenCount: GetNonNegativeInput<int>("children count")
		);

		var taxes = new Tax[]
		{
			new AbroadTransfersTax(data),
			new GiftsTax(data),
			new GoodsTax(data),
			new JobTax(data),
			new MilitaryTax(data)
		};

		dataCollection[name] = taxes;
	}

	static void Files()
	{
		var dirs = Directory.GetDirectories(DataDirectory);
		if (!dirs.Any())
		{
			Console.WriteLine("Data directory is empty");
			return;
		}

		foreach (var dirPath in dirs)
		{
			string name = Path.GetFileName(dirPath);
			Console.WriteLine($"-> {name}");
		}
	}

	static void Write()
	{
		Console.Write("Enter name: ");
		string name = Console.ReadLine()!;

		if (!dataCollection.ContainsKey(name))
		{
			Console.WriteLine("There is no data associated with this name");
			return;
		}

		string directoryPath = Path.Combine(DataDirectory, $"{name}");
		Directory.CreateDirectory(directoryPath);

		foreach (var tax in dataCollection[name])
		{
			string filePath = Path.Combine(directoryPath, $"{tax.Name}.json");
			using var s = File.Create(filePath);
			JsonSerializer.Serialize(s, tax, new JsonSerializerOptions() { WriteIndented = true });
		}

		Process.Start("explorer.exe", directoryPath);
	}

	static void Read()
	{
		Console.Write("Enter name: ");
		string name = Console.ReadLine()!;

		string directoryPath = Path.Combine(DataDirectory, name);
		if (!Directory.Exists(directoryPath))
		{
			Console.WriteLine("There is no data directory with this name");
			return;
		}

		var taxes = new List<Tax>();
		foreach (string filePath in Directory.EnumerateFiles(directoryPath))
		{
			using var s = File.OpenRead(filePath);
			var tax = JsonSerializer.Deserialize<Tax>(s)!;
			taxes.Add(tax);
		}

		dataCollection[name] = taxes;
	}

	static void Main()
	{
		while (true)
		{
			foreach (var (c, (_, description)) in actions)
			{
				Console.WriteLine($"Enter {c} to {description}");
			}
			Console.WriteLine();

			char choice = GetInput<char>("your choice", actions.ContainsKey);
			Console.WriteLine();

			actions[choice].Action();

			Console.WriteLine();
			Console.Write("Press enter to continue...");
			Console.ReadLine();
			Console.Clear();
		}
	}
}