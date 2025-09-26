using TNetSdk;

public class COMA_CD_PlayerBuff : COMA_CommandDatas
{
	public COMA_Buff.Buff buffState;

	public float buffTime;

	public string buffName = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("b", (byte)buffState);
		obj.PutFloat("bt", buffTime);
		obj.PutUtfString("bn", buffName);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		buffState = (COMA_Buff.Buff)obj.GetByte("b");
		buffTime = obj.GetFloat("bt");
		buffName = obj.GetUtfString("bn");
		base.Unpackage(obj);
	}
}
