using MessageID;
using UnityEngine;

public class UIBackpack_ButtonMinusPrice : MonoBehaviour
{
	private float _pressTime = -1f;

	private void Start()
	{
	}

	private void Update()
	{
		if (_pressTime > 0f && Time.time - _pressTime > 3f)
		{
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIBackpack_SellItemMinusPrice, null, 1000);
			_pressTime = -1f;
		}
	}

	private void OnClick()
	{
		_pressTime = -1f;
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIBackpack_SellItemMinusPrice, null, null);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Figure);
	}
}
