using UnityEngine;

public class UIRanking_RankIcon : MonoBehaviour
{
	[SerializeField]
	private GameObject[] rankingIcons;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetRankId(int nRank, GameObject objNum)
	{
		int num = rankingIcons.Length;
		if (nRank <= num)
		{
			objNum.SetActive(false);
			base.gameObject.SetActive(true);
			for (int i = 0; i < num; i++)
			{
				if (i == nRank - 1)
				{
					rankingIcons[i].SetActive(true);
				}
				else
				{
					rankingIcons[i].SetActive(false);
				}
			}
		}
		else
		{
			objNum.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}
}
