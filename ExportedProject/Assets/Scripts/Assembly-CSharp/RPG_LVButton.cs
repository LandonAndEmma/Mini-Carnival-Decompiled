using UnityEngine;

public class RPG_LVButton : MonoBehaviour
{
	[SerializeField]
	private UIRPG_SettlementMgr _mgr;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		_mgr.CloseLvBox();
	}
}
