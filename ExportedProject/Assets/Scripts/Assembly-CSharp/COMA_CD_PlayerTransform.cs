using TNetSdk;
using UnityEngine;

public class COMA_CD_PlayerTransform : COMA_CommandDatas
{
	public Vector3 position = Vector3.zero;

	public Quaternion rotation = Quaternion.identity;

	public byte btElevation;

	public override void Package(ref SFSObject obj)
	{
		obj.PutFloatArray("p", new float[3] { position.x, position.y, position.z });
		obj.PutFloat("r", rotation.eulerAngles.y);
		obj.PutByte("e", btElevation);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		float[] floatArray = obj.GetFloatArray("p");
		position = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		float y = obj.GetFloat("r");
		rotation.eulerAngles = new Vector3(0f, y, 0f);
		btElevation = obj.GetByte("e");
		base.Unpackage(obj);
	}
}
