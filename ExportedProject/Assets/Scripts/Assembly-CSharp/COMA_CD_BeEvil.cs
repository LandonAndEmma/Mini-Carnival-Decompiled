using TNetSdk;

public class COMA_CD_BeEvil : COMA_CommandDatas
{
	public bool bEvil;

	public override void Package(ref SFSObject obj)
	{
		obj.PutBool("e", bEvil);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		bEvil = obj.GetBool("e");
		base.Unpackage(obj);
	}
}
