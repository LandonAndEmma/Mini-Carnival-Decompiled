namespace TNetSdk
{
	public class RoomReadyReqCmd : RoomCmd
	{
		public Packet MakePacket()
		{
			return MakePacket(CMD.room_user_ready);
		}
	}
}
