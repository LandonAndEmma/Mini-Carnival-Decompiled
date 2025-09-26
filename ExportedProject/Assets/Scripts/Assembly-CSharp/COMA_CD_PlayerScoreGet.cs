using TNetSdk;

public class COMA_CD_PlayerScoreGet : COMA_CommandDatas
{
	public int playerId;

	public int curScore;

	public int addScore;

	public override void Package(ref SFSObject obj)
	{
		obj.PutInt("id", playerId);
		obj.PutInt("s", curScore);
		obj.PutInt("a", addScore);
		base.Package(ref obj);
	}

	public override void Unpackage(SFSObject obj)
	{
		playerId = obj.GetInt("id");
		curScore = obj.GetInt("s");
		addScore = obj.GetInt("a");
		base.Unpackage(obj);
	}
}
