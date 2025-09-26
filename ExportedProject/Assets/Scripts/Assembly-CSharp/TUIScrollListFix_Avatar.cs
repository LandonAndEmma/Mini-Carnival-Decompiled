using UnityEngine;

public class TUIScrollListFix_Avatar : TUIControlImpl
{
	public TUIControl[] sencesControls;

	public float spacing = 10f;

	private float _perBtnLen = 70f;

	private bool _bFixX;

	private void Start()
	{
	}

	private void Update()
	{
		if (!_bFixX)
		{
			Vector3 position = base.transform.position;
			position.x = 0f;
			base.transform.position = position;
			_bFixX = true;
		}
	}

	public void InitScrollList(TUIControl[] contr)
	{
		sencesControls = new TUIControl[contr.Length];
		for (int i = 0; i < contr.Length; i++)
		{
			sencesControls[i] = contr[i];
		}
		float num = (float)contr.Length * _perBtnLen + (float)(contr.Length - 1) * spacing;
		if (num > size.x)
		{
			Debug.LogError("==================Error1");
			return;
		}
		float num2 = (size.x - num) / 2f;
		num2 += (0f - size.x) / 2f;
		float num3 = num2 + _perBtnLen / 2f;
		for (int j = 0; j < contr.Length; j++)
		{
			sencesControls[j].gameObject.transform.parent = base.transform;
			Vector3 localPosition = sencesControls[j].transform.localPosition;
			localPosition.x = num3;
			localPosition.y = 0f;
			localPosition.z = 0f;
			sencesControls[j].transform.localPosition = localPosition;
			num3 += spacing + _perBtnLen;
		}
	}
}
