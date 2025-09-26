using TNetSdk.BinaryProtocol;

namespace TNetSdk
{
	public class RoomGoNotifyCmd : UnPacker
	{
		public override bool ParserPacket(Packet packet)
		{
			if (!base.ParserPacket(packet))
			{
				return false;
			}
			return true;
		}
	}
}
