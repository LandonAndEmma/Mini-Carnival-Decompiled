using UnityEngine;

public class UIRPGRoundMgr : MonoBehaviour
{
	[SerializeField]
	private UILabel _lableRoundNum;

	[SerializeField]
	private Animation _ani;

	private void Awake()
	{
		_lableRoundNum.enabled = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RoundChange(int nRound)
	{
		_lableRoundNum.enabled = true;
		_lableRoundNum.text = nRound.ToString();
		_ani.Play();
	}
}
