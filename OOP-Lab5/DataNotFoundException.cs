namespace OOP_Lab5;

public class DataNotFoundException : Exception
{
	public DataNotFoundException() : base("Specified data is not found")
	{
	}
}
