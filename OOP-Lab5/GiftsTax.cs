namespace OOP_Lab5;

public class GiftsTax : Tax
{
	public GiftsTax() { }
	public GiftsTax(TaxpayerData taxpayerData) : base(taxpayerData) { }

	protected override double Percent => 18.0;
	public override string Name => "Gifts";

	protected override double GetTaxedAmount(TaxpayerData taxpayerData)
		=> taxpayerData.MoneyGiftsAmount + taxpayerData.GoodsGiftsAmount;
}
