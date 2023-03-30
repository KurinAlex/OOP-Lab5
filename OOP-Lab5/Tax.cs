using System.Text.Json.Serialization;

namespace OOP_Lab5;

[JsonDerivedType(typeof(AbroadTransfersTax), 1)]
[JsonDerivedType(typeof(GiftsTax), 2)]
[JsonDerivedType(typeof(GoodsTax), 3)]
[JsonDerivedType(typeof(JobTax), 4)]
[JsonDerivedType(typeof(MilitaryTax), 5)]
public abstract class Tax
{
	public Tax() { }

	public Tax(TaxpayerData taxpayerData)
	{
		if (Percent < 0.0)
		{
			throw new NegativePercentExcpetion();
		}
		double taxedAmount = GetTaxedAmount(taxpayerData);
		Amount = Math.Max(0.0, taxedAmount * Percent / 100.0);
	}

	public double Amount { get; init; }
	public abstract string Name { get; }
	protected abstract double Percent { get; }

	protected abstract double GetTaxedAmount(TaxpayerData taxpayerData);
	public override string ToString() => $"{Name} tax: {Amount}";
}
