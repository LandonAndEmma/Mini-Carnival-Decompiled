using UnityEngine;

public class PraiseBtn : BillBoard
{
	[SerializeField]
	public Transform _iconTrans;

	[SerializeField]
	public COMA_PlayerSync_Fishing _fishPlayer;

	private void Start()
	{
		SetCamera(GameObject.Find("Main Camera").transform);
		SetType(EBillBoardType.CTC);
	}

	private new void Update()
	{
		base.Update();
	}
}
