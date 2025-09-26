using UnityEngine;

public class TUIScaleAnim : TUIValueAnim
{
	private void Start()
	{
		base.transform.localScale = new Vector3(m_fBeginValue, m_fBeginValue, m_fBeginValue);
	}

	protected override void OnValueUpdate(float value)
	{
		base.transform.localScale = new Vector3(value, value, value);
	}
}
