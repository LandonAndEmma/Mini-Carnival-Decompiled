using TNetSdk;
using UnityEngine;

public class COMA_CD_PlayerAnimation : COMA_CommandDatas
{
	public byte btAnimName;

	public byte btFadeTime = 30;

	public byte btPlaySeed = 10;

	public Vector3 extra = Vector3.zero;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("a", btAnimName);
		obj.PutByte("f", btFadeTime);
		obj.PutByte("p", btPlaySeed);
		obj.PutFloat("px", extra.x);
		obj.PutFloat("py", extra.y);
		obj.PutFloat("pz", extra.z);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		btAnimName = obj.GetByte("a");
		btFadeTime = obj.GetByte("f");
		btPlaySeed = obj.GetByte("p");
		extra.x = obj.GetFloat("px");
		extra.y = obj.GetFloat("py");
		extra.z = obj.GetFloat("pz");
		base.Unpackage(obj);
	}
}
