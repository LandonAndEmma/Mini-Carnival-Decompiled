using UnityEngine;

public class UIInGame_RunGoldNum : MonoBehaviour
{
	[SerializeField]
	private TUILabel _labelNum;

	public int GoldNum
	{
		set
		{
			_labelNum.Text = value.ToString();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			GoldNum = COMA_PlayerSelf.Instance.goldGet;
		}
	}
}
