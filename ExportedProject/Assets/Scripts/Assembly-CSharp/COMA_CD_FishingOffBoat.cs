using TNetSdk;

public class COMA_CD_FishingOffBoat : COMA_CommandDatas
{
	public string boatId = "0";

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("i", boatId);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		boatId = obj.GetUtfString("i");
		base.Unpackage(obj);
	}
}
