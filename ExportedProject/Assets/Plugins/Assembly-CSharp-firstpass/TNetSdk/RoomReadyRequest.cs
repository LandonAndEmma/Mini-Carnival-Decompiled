namespace TNetSdk
{
	public class RoomReadyRequest : TNetRequest
	{
		public RoomReadyRequest()
			: base(RequestType.RoomUserReady)
		{
			Init();
		}

		private void Init()
		{
			packer = new RoomReadyReqCmd();
			packet = ((RoomReadyReqCmd)packer).MakePacket();
		}
	}
}
