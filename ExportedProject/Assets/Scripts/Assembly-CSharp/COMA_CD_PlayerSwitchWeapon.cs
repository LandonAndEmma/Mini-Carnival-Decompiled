using TNetSdk;

public class COMA_CD_PlayerSwitchWeapon : COMA_CommandDatas
{
	public string weaponSerialName = "W001";

	public override void Package(ref SFSObject obj)
	{
		obj.PutUtfString("w", weaponSerialName);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		weaponSerialName = obj.GetUtfString("w");
		base.Unpackage(obj);
	}
}
