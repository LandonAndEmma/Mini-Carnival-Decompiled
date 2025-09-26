namespace Protocol
{
	public abstract class BaseCmd
	{
		public abstract void Serialize(BufferWriter writer);
	}
}
