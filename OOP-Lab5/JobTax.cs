namespace OOP_Lab5;

public class JobTax : Tax
{
	private const double _maxBenefitedAmount = 2684.0;
	private const double _benefitAmount = 0.5 * _maxBenefitedAmount;

	public JobTax() { }
	public JobTax(TaxpayerData incomeData) : base(incomeData) { }

	protected override double Percent => 15.0;
	public override string Name => "Job";

	protected override double GetTaxedAmount(TaxpayerData taxpayerData)
	{
		double amount = taxpayerData.MainJobIncome + taxpayerData.AdditionalJobIncome;
		if (amount <= _maxBenefitedAmount)
		{
			amount -= _benefitAmount;
		}
		if (taxpayerData.ChildrenCount > 1)
		{
			amount -= _benefitAmount * taxpayerData.ChildrenCount;
		}
		return amount;
	}
}
