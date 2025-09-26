using MessageID;
using UnityEngine;

public class UIMarket_ButtonFavorites : MonoBehaviour
{
	public enum State
	{
		UnCollected = 0,
		Collected = 1
	}

	[SerializeField]
	private UISprite _collectedSprite;

	[SerializeField]
	private UISprite _uncollectedSprite;

	private State _state;

	public State BtnState
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			_collectedSprite.enabled = _state == State.Collected;
			_uncollectedSprite.enabled = ((_state != State.Collected) ? true : false);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Collect);
		if (BtnState == State.Collected)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_UncollectCurAvatar, null, null);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_CollectCurAvatar, null, null);
		}
	}
}
