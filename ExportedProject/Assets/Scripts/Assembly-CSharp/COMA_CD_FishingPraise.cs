using TNetSdk;

public class COMA_CD_FishingPraise : COMA_CommandDatas
{
	public int itemID = -1;

	public int nId = -1;

	public string sendName = string.Empty;

	public string recvName = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutInt("i", itemID);
		obj.PutInt("pd", nId);
		obj.PutUtfString("sn", sendName);
		obj.PutUtfString("rn", recvName);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		itemID = obj.GetInt("i");
		nId = obj.GetInt("pd");
		sendName = obj.GetUtfString("sn");
		recvName = obj.GetUtfString("rn");
		base.Unpackage(obj);
	}
}
