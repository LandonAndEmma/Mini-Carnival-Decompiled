using TNetSdk;

public class COMA_CD_DeleteBlock : COMA_CommandDatas
{
	public byte id;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("i", id);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		id = obj.GetByte("i");
		base.Unpackage(obj);
	}
}
