using TNetSdk;

public class COMA_CD_PlayerTextureInit : COMA_CommandDatas
{
	public string[] texUUID = new string[3];

	public string[] texStr = new string[3];

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("t0", texUUID[0]);
		obj.PutUtfString("t1", texUUID[1]);
		obj.PutUtfString("t2", texUUID[2]);
		obj.PutUtfString("tex0", texStr[0]);
		obj.PutUtfString("tex1", texStr[1]);
		obj.PutUtfString("tex2", texStr[2]);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		texUUID[0] = obj.GetUtfString("t0");
		texUUID[1] = obj.GetUtfString("t1");
		texUUID[2] = obj.GetUtfString("t2");
		texStr[0] = obj.GetUtfString("tex0");
		texStr[1] = obj.GetUtfString("tex1");
		texStr[2] = obj.GetUtfString("tex2");
		base.Unpackage(obj);
	}
}
