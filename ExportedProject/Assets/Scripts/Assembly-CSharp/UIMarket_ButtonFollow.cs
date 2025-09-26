using MessageID;
using UnityEngine;

public class UIMarket_ButtonFollow : MonoBehaviour
{
	public enum State
	{
		UnFollow = 0,
		Followed = 1
	}

	[SerializeField]
	private UILabel _btnLabel;

	private State _state;

	[SerializeField]
	private UIMarket_AuthorDetail authorDetail;

	public State BtnState
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			_btnLabel.text = ((_state != State.UnFollow) ? "UNFOLLOW" : "FOLLOW");
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
		Debug.Log("------Btn Follow Click:" + BtnState);
		if (BtnState == State.Followed)
		{
			if (authorDetail == null)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_UnfollowPlayer, null, null);
			}
			else
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_UnfollowPlayer, null, authorDetail.CurAuthorId);
			}
		}
		else if (authorDetail == null)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_FollowPlayer, null, null);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_FollowPlayer, null, authorDetail.CurAuthorId);
		}
	}
}
