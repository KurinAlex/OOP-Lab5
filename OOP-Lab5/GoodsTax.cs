namespace OOP_Lab5;

public class GoodsTax : Tax
{
	public GoodsTax() { }
	public GoodsTax(TaxpayerData taxpayerData) : base(taxpayerData) { }

	protected override double Percent => 0.5;
	public override string Name => "Goods";

	protected override double GetTaxedAmount(TaxpayerData taxpayerData)
		=> taxpayerData.GoodsGiftsAmount + taxpayerData.GoodsSellsIncome;
}
