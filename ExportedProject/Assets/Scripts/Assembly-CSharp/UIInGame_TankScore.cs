using UnityEngine;

public class UIInGame_TankScore : MonoBehaviour
{
	public TUILabel team1Score;

	public TUILabel team2Score;

	public TUILabel killCount;

	public TUILabel dieCount;

	private bool bProccessed;

	private void Start()
	{
	}

	private bool changeScorePos()
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			if (TankCommon.getTeamIndex(COMA_PlayerSelf.Instance.sitIndex) != 0)
			{
				Debug.Log("switch score label!------");
				Vector3 position = team1Score.transform.position;
				team1Score.transform.position = team2Score.transform.position;
				team2Score.transform.position = position;
			}
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (COMA_Tank_SceneController.Instance != null)
		{
			int ourScore = COMA_Tank_SceneController.Instance.getOurScore();
			int oppScore = COMA_Tank_SceneController.Instance.getOppScore();
			team1Score.Text = ourScore.ToString();
			team2Score.Text = oppScore.ToString();
			dieCount.Text = COMA_Tank_SceneController.Instance._nDieCount.ToString();
			killCount.Text = COMA_Tank_SceneController.Instance._nKillCount.ToString();
		}
	}
}
