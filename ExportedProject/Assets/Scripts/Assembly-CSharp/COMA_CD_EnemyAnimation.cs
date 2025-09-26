using TNetSdk;

public class COMA_CD_EnemyAnimation : COMA_CommandDatas
{
	public byte bEnemyID;

	public byte btAnimName;

	public byte btFadeTime = 30;

	public byte btPlaySeed = 10;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("i", bEnemyID);
		obj.PutByte("a", btAnimName);
		obj.PutByte("f", btFadeTime);
		obj.PutByte("p", btPlaySeed);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		bEnemyID = obj.GetByte("i");
		btAnimName = obj.GetByte("a");
		btFadeTime = obj.GetByte("f");
		btPlaySeed = obj.GetByte("p");
		base.Unpackage(obj);
	}
}
