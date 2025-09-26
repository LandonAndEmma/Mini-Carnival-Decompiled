using UnityEngine;

public class UIRPG_PickCard : MonoBehaviour
{
	[SerializeField]
	private UIRPGPickCards _pickCardsMgr;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AniEnd()
	{
		_pickCardsMgr.PickCardsAniEnd();
	}
}
