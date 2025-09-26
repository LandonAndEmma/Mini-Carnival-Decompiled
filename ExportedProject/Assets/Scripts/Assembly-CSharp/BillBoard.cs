using UnityEngine;

public class BillBoard : MonoBehaviour
{
	public enum EBillBoardType
	{
		CTC = 0
	}

	[SerializeField]
	private Transform _cameraTo;

	private EBillBoardType _curType;

	public void SetType(EBillBoardType type)
	{
		_curType = type;
	}

	public new EBillBoardType GetType()
	{
		return _curType;
	}

	public void SetCamera(Transform cam)
	{
		_cameraTo = cam;
	}

	private void Start()
	{
	}

	public void Update()
	{
		if (_curType == EBillBoardType.CTC)
		{
			base.transform.LookAt(_cameraTo.position);
		}
	}
}
