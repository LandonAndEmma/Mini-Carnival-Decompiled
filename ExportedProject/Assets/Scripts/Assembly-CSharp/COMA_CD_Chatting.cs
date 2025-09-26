using TNetSdk;

public class COMA_CD_Chatting : COMA_CommandDatas
{
	public string chatting = string.Empty;

	public string sendName = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("cc", chatting);
		obj.PutUtfString("sn", sendName);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		chatting = obj.GetUtfString("cc");
		sendName = obj.GetUtfString("sn");
		base.Unpackage(obj);
	}
}
