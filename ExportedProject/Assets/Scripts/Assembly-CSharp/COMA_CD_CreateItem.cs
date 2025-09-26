using TNetSdk;

public class COMA_CD_CreateItem : COMA_CommandDatas
{
	public byte blockIndex;

	public byte itemIndex;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("b", blockIndex);
		obj.PutByte("i", itemIndex);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		blockIndex = obj.GetByte("b");
		itemIndex = obj.GetByte("i");
		base.Unpackage(obj);
	}
}
