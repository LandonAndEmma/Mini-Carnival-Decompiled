public class TankCommon
{
	public static int nPlayerCount = 4;

	public static bool isAliance(int nMe, int nOther)
	{
		if (nPlayerCount == 2)
		{
			return nMe == nOther;
		}
		return (nMe >= nPlayerCount / 2) ? (nOther >= nPlayerCount / 2) : (nOther < nPlayerCount / 2);
	}

	public static int getTeamIndex(int nSitId)
	{
		return (nSitId >= nPlayerCount / 2) ? 1 : 0;
	}

	public static bool isBlueTeam(int nSitId)
	{
		return getTeamIndex(nSitId) == 0;
	}
}
