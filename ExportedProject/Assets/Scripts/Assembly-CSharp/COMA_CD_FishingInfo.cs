using TNetSdk;

public class COMA_CD_FishingInfo : COMA_CommandDatas
{
	public int itemID = -1;

	public int nFishWeight;

	public string strId = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutInt("i", itemID);
		obj.PutInt("fw", nFishWeight);
		obj.PutUtfString("pid", strId);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		itemID = obj.GetInt("i");
		nFishWeight = obj.GetInt("fw");
		strId = obj.GetUtfString("pid");
		base.Unpackage(obj);
	}
}
