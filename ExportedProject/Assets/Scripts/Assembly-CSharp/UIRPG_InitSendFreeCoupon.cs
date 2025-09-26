using UnityEngine;

public class UIRPG_InitSendFreeCoupon : MonoBehaviour
{
	[SerializeField]
	private UILabel _numLabel;

	[SerializeField]
	private GameObject _firstLoginAward;

	private void OnEnable()
	{
		_firstLoginAward.SetActive(false);
		if (!COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.RPGFirstLoginAward_PerDay >= 0)
		{
			_firstLoginAward.SetActive(true);
			UIDataBufferCenter.Instance.RPGFirstLoginAward_PerDay = -1;
		}
	}

	public void Start()
	{
		_numLabel.text = RPGGlobalData.Instance.RpgMiscUnit._cardNum_donate.ToString();
	}
}
