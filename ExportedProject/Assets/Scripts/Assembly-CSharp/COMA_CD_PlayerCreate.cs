using TNetSdk;
using UnityEngine;

public class COMA_CD_PlayerCreate : COMA_CommandDatas
{
	public string GID = string.Empty;

	public Vector3 position = Vector3.zero;

	public Quaternion rotation = Quaternion.identity;

	public byte bNeedRecreate = 1;

	public string nickname = string.Empty;

	public int lv;

	public int exp;

	public int rankscore;

	public string[] texUUID = new string[3];

	public string[] accSerial = new string[7];

	public int itemSelected;

	public override void Package(ref SFSObject obj)
	{
		obj.PutFloatArray("p", new float[3] { position.x, position.y, position.z });
		obj.PutFloat("r", rotation.eulerAngles.y);
		obj.PutByte("rc", bNeedRecreate);
		obj.PutUtfString("n", nickname);
		obj.PutInt("l", lv);
		obj.PutInt("e", exp);
		obj.PutInt("rs", rankscore);
		obj.PutUtfString("t0", texUUID[0]);
		obj.PutUtfString("t1", texUUID[1]);
		obj.PutUtfString("t2", texUUID[2]);
		obj.PutUtfString("a0", accSerial[0]);
		obj.PutUtfString("a1", accSerial[1]);
		obj.PutUtfString("a2", accSerial[2]);
		obj.PutUtfString("a3", accSerial[3]);
		obj.PutUtfString("a4", accSerial[4]);
		obj.PutUtfString("a5", accSerial[5]);
		obj.PutUtfString("a6", accSerial[6]);
		obj.PutUtfString("a7", GID);
		obj.PutInt("it", itemSelected);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		float[] floatArray = obj.GetFloatArray("p");
		position = new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		float y = obj.GetFloat("r");
		rotation.eulerAngles = new Vector3(0f, y, 0f);
		bNeedRecreate = obj.GetByte("rc");
		nickname = obj.GetUtfString("n");
		lv = obj.GetInt("l");
		exp = obj.GetInt("e");
		rankscore = obj.GetInt("rs");
		texUUID[0] = obj.GetUtfString("t0");
		texUUID[1] = obj.GetUtfString("t1");
		texUUID[2] = obj.GetUtfString("t2");
		accSerial[0] = obj.GetUtfString("a0");
		accSerial[1] = obj.GetUtfString("a1");
		accSerial[2] = obj.GetUtfString("a2");
		accSerial[3] = obj.GetUtfString("a3");
		accSerial[4] = obj.GetUtfString("a4");
		accSerial[5] = obj.GetUtfString("a5");
		accSerial[6] = obj.GetUtfString("a6");
		GID = obj.GetUtfString("a7");
		itemSelected = obj.GetInt("it");
		base.Unpackage(obj);
	}
}
