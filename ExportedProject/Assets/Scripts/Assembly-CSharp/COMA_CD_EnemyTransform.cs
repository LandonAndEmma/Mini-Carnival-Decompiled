using TNetSdk;
using UnityEngine;

public class COMA_CD_EnemyTransform : COMA_CommandDatas
{
	public byte bEnemyID;

	public Vector3 position = Vector3.zero;

	public Quaternion rotation = Quaternion.identity;

	public override void Package(ref SFSObject obj)
	{
		obj.PutByte("i", bEnemyID);
		obj.PutFloatArray("p", new float[3] { position.x, position.y, position.z });
		obj.PutFloat("r", rotation.eulerAngles.y);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		bEnemyID = obj.GetByte("i");
		float[] floatArray = obj.GetFloatArray("p");
		position = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		float y = obj.GetFloat("r");
		rotation.eulerAngles = new Vector3(0f, y, 0f);
		base.Unpackage(obj);
	}
}
