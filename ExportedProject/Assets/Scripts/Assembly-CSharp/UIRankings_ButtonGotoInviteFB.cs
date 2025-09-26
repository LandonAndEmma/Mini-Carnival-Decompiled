using UnityEngine;

public class UIRankings_ButtonGotoInviteFB : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIFacebookFeedback.Instance.InviteFriends();
	}
}
