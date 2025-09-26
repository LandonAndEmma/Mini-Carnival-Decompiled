using TNetSdk;

public class COMA_CD_PlayerHPSet : COMA_CommandDatas
{
	public float hp;

	public override void Package(ref SFSObject obj)
	{
		obj.PutFloat("h", hp);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		hp = obj.GetFloat("h");
		base.Unpackage(obj);
	}
}
