using TNetSdk;

public class COMA_CD_UseItemInfo : COMA_CommandDatas
{
	public string nameA = string.Empty;

	public int itemID = -1;

	public string nameB = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("a", nameA);
		obj.PutInt("i", itemID);
		obj.PutUtfString("b", nameB);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		nameA = obj.GetUtfString("a");
		itemID = obj.GetInt("i");
		nameB = obj.GetUtfString("b");
		base.Unpackage(obj);
	}
}
