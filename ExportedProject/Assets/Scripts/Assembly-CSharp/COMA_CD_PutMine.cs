using TNetSdk;
using UnityEngine;

public class COMA_CD_PutMine : COMA_CommandDatas
{
	public Vector3 pos = Vector3.zero;

	public string strMineName = string.Empty;

	public override void Package(ref SFSObject obj)
	{
		obj.PutFloatArray("p", new float[3] { pos.x, pos.y, pos.z });
		obj.PutUtfString("mn", strMineName);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		float[] floatArray = obj.GetFloatArray("p");
		pos = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		strMineName = obj.GetUtfString("mn");
		base.Unpackage(obj);
	}
}
