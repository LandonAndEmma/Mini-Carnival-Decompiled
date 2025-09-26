using UnityEngine;

public class UI_GameItemLinkTurn : MonoBehaviour
{
	[SerializeField]
	private UI_GameRuleWeaponSelGroup selGroup;

	[SerializeField]
	private Animation _linkTurn;

	[SerializeField]
	private AnimationClip _linkClip;

	private int _nState;

	[SerializeField]
	private int _nIndex;

	public int nState
	{
		get
		{
			return _nState;
		}
		set
		{
			_nState = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void LinkTurn(int state)
	{
		Debug.Log("Link turn : " + state + "_nIndex:" + _nIndex);
		if (_linkTurn != null)
		{
			UI_GameItemLinkTurn component = _linkTurn.GetComponent<UI_GameItemLinkTurn>();
			if (component != null)
			{
				component.nState = state;
			}
			if (_linkClip == null)
			{
				_linkTurn.Play();
			}
			else
			{
				_linkTurn.Play(_linkClip.name);
			}
		}
	}

	public void LinkAniEnd(int n)
	{
		if (selGroup != null)
		{
			selGroup.ChangeTurnState(_nIndex, (UI_GameRuleWeaponSelGroup.ETurnState)_nState);
		}
	}
}
