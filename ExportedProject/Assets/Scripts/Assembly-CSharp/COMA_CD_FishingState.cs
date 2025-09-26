using TNetSdk;

public class COMA_CD_FishingState : COMA_CommandDatas
{
	public int nState;

	public int nFishBoatID;

	public float fOnBoatDurTime;

	public override void Package(ref SFSObject obj)
	{
		obj.PutInt("s", nState);
		obj.PutInt("fb", nFishBoatID);
		obj.PutFloat("dt", fOnBoatDurTime);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		nState = obj.GetInt("s");
		nFishBoatID = obj.GetInt("fb");
		fOnBoatDurTime = obj.GetFloat("dt");
		base.Unpackage(obj);
	}
}
