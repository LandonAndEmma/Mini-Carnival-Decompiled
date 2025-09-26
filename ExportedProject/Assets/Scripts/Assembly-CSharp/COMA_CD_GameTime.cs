using TNetSdk;

public class COMA_CD_GameTime : COMA_CommandDatas
{
	public float timeLast;

	public override void Package(ref SFSObject obj)
	{
		obj.PutFloat("t", timeLast);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		timeLast = obj.GetFloat("t");
		base.Unpackage(obj);
	}
}
