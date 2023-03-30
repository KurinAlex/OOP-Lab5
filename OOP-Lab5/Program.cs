using System.Text.Json;

namespace OOP_Lab5;

public class Program
{
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
		string name = InputHelper.GetStringInput("name", s => !string.IsNullOrEmpty(s));

		var data = new TaxpayerData(
			MainJobIncome: InputHelper.GetNonNegativeInput<double>("main job income"),
			AdditionalJobIncome: InputHelper.GetNonNegativeInput<double>("additional job income"),
			AuthorAwardsAmount: InputHelper.GetNonNegativeInput<double>("author awards amount"),
			GoodsSellsIncome: InputHelper.GetNonNegativeInput<double>("goods sells income"),
			MoneyGiftsAmount: InputHelper.GetNonNegativeInput<double>("money gifts amount"),
			GoodsGiftsAmount: InputHelper.GetNonNegativeInput<double>("goods gifts amount"),
			AbroadTransfersAmount: InputHelper.GetNonNegativeInput<double>("abroad transfers amount"),
			ChildrenCount: InputHelper.GetNonNegativeInput<int>("children count")
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
			throw new DataNotFoundException();
		}

		string directoryPath = Path.Combine(DataDirectory, $"{name}");
		Directory.CreateDirectory(directoryPath);

		foreach (var tax in dataCollection[name])
		{
			string filePath = Path.Combine(directoryPath, $"{tax.Name}.json");
			using var s = File.Create(filePath);
			JsonSerializer.Serialize(s, tax, new JsonSerializerOptions() { WriteIndented = true });
		}
	}

	static void Read()
	{
		Console.Write("Enter name: ");
		string name = Console.ReadLine()!;

		string directoryPath = Path.Combine(DataDirectory, name);
		if (!Directory.Exists(directoryPath))
		{
			throw new DataNotFoundException();
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

			char choice = InputHelper.GetInput<char>("your choice", actions.ContainsKey);
			Console.WriteLine();

			try
			{
				actions[choice].Action();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine();
			Console.Write("Press enter to continue...");
			Console.ReadLine();
			Console.Clear();
		}
	}
}