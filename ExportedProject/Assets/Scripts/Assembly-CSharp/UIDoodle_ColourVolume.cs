using UnityEngine;

public class UIDoodle_ColourVolume : MonoBehaviour
{
	public UIDoodle_ColourContainer container;

	public GameObject objVolumeIndicate;

	public GameObject objVolumeLabelIndicate;

	private float maxLength = 149f;

	public bool bTest;

	public float fRate;

	private void Start()
	{
	}

	private void Update()
	{
		if (bTest)
		{
			UIDoodle_ColourBoxSlot curSelSlot = container.CurSelSlot;
			if (curSelSlot != null)
			{
				curSelSlot.Volume = fRate;
			}
			bTest = false;
		}
	}

	public void RefreshVolume(float f)
	{
		f = Mathf.Clamp01(f);
		Vector3 localScale = objVolumeIndicate.transform.localScale;
		localScale.y = f;
		objVolumeIndicate.transform.localScale = localScale;
		float num = (1f - f) / 2f;
		float y = (0f - num) * maxLength;
		Vector3 localPosition = objVolumeIndicate.transform.localPosition;
		localPosition.y = y;
		objVolumeIndicate.transform.localPosition = localPosition;
		int num2 = (int)(f * 100f);
		objVolumeLabelIndicate.GetComponent<TUILabel>().Text = num2 + "%";
	}
}
