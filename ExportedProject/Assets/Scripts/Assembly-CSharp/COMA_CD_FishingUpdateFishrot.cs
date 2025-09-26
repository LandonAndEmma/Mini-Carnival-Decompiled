using TNetSdk;

public class COMA_CD_FishingUpdateFishrot : COMA_CommandDatas
{
	public byte btType;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("e", btType);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		btType = obj.GetByte("e");
		base.Unpackage(obj);
	}
}
