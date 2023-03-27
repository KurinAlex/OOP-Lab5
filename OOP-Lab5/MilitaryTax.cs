namespace OOP_Lab5;

public class MilitaryTax : Tax
{
	public MilitaryTax() { }
	public MilitaryTax(TaxpayerData taxpayerData) : base(taxpayerData) { }

	protected override double Percent => 1.5;
	public override string Name => "Military";

	protected override double GetTaxedAmount(TaxpayerData taxpayerData)
		=> taxpayerData.MainJobIncome + taxpayerData.AdditionalJobIncome + taxpayerData.AbroadTransfersAmount;
}
