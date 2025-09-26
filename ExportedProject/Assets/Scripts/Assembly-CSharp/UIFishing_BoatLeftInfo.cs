using UnityEngine;

public class UIFishing_BoatLeftInfo : MonoBehaviour
{
	[SerializeField]
	private TUILabel _labelTime;

	private bool bNeedNotify;

	[SerializeField]
	private Animation _aniClock;

	private bool bStartAni;

	protected void OnEnable()
	{
		bNeedNotify = true;
		_aniClock.Stop();
		_aniClock.gameObject.transform.FindChild("clock_m").localRotation = Quaternion.Euler(Vector3.zero);
		bStartAni = false;
	}

	protected void OnDisable()
	{
		bNeedNotify = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		int num = ((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).GetOnBoatId() - 1;
		if (num >= 0)
		{
			_labelTime.Text = COMA_Fishing_SceneController.Instance.GetFormatBoatLeftTime(num);
		}
		if (bNeedNotify && _labelTime.Text == "00:00")
		{
			int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1013, TTelegram.SEND_MSG_IMMEDIATELY, null);
			bNeedNotify = false;
		}
		if (num >= 0 && !bStartAni && COMA_Fishing_SceneController.Instance.GetBoatLeftTime(num) <= 10f)
		{
			_aniClock.Play();
			bStartAni = true;
		}
	}
}
