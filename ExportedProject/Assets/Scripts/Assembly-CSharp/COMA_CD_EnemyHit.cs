using TNetSdk;
using UnityEngine;

public class COMA_CD_EnemyHit : COMA_CommandDatas
{
	public byte bEnemyID;

	public float attackPoint;

	public Vector3 blastPush = Vector3.zero;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("i", bEnemyID);
		obj.PutFloat("ap", attackPoint);
		obj.PutFloat("px", blastPush.x);
		obj.PutFloat("py", blastPush.y);
		obj.PutFloat("pz", blastPush.z);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		bEnemyID = obj.GetByte("i");
		attackPoint = obj.GetFloat("ap");
		blastPush.x = obj.GetFloat("px");
		blastPush.y = obj.GetFloat("py");
		blastPush.z = obj.GetFloat("pz");
		base.Unpackage(obj);
	}
}
