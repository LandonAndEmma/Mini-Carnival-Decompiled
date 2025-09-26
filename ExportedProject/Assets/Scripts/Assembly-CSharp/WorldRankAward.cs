public class WorldRankAward
{
	public enum Type
	{
		None = 0,
		Gold = 1,
		Crystal = 2,
		Heart = 3
	}

	public string _accessoriesSerial;

	public Type _type;

	public int _num;

	public WorldRankAward(string accessoriesSerial, int type, int num)
	{
		_accessoriesSerial = accessoriesSerial;
		_type = (Type)(0 + type);
		_num = num;
	}
}
