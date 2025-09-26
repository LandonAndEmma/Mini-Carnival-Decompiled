using TNetSdk;

public class COMA_CD_CreateRoad : COMA_CommandDatas
{
	public string rIDs = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("r", rIDs);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		rIDs = obj.GetUtfString("r");
		base.Unpackage(obj);
	}
}
