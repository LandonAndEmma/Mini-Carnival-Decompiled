using TNetSdk;

public class COMA_CD_FishingOnBoat : COMA_CommandDatas
{
	public string boatId = "0";

	public float onBoatTime;

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("i", boatId);
		obj.PutFloat("obt", onBoatTime);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		boatId = obj.GetUtfString("i");
		onBoatTime = obj.GetFloat("obt");
		base.Unpackage(obj);
	}
}
