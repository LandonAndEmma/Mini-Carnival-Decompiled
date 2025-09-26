using TNetSdk;
using UnityEngine;

public class COMA_CD_PlayerHit : COMA_CommandDatas
{
	public byte btBulletKind;

	public int nFromID;

	public float attackPoint;

	public Vector3 blastPush = Vector3.zero;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("b", btBulletKind);
		obj.PutInt("fr", nFromID);
		obj.PutFloat("ap", attackPoint);
		obj.PutFloat("px", blastPush.x);
		obj.PutFloat("py", blastPush.y);
		obj.PutFloat("pz", blastPush.z);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		btBulletKind = obj.GetByte("b");
		nFromID = obj.GetInt("fr");
		attackPoint = obj.GetFloat("ap");
		blastPush.x = obj.GetFloat("px");
		blastPush.y = obj.GetFloat("py");
		blastPush.z = obj.GetFloat("pz");
		base.Unpackage(obj);
	}
}
