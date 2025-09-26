using UnityEngine;

public class UIFishing_OffBoatCheck : MonoBehaviour
{
	[SerializeField]
	private TUILabel _labelTime;

	private bool bCount;

	private int nIndex = -1;

	public void InitOffBoatCheckId(int boatID)
	{
		nIndex = boatID;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (bCount)
		{
			_labelTime.Text = COMA_Fishing_SceneController.Instance.GetFormatBoatLeftTime(nIndex);
		}
	}

	private void OnEnable()
	{
		bCount = true;
	}

	private void OnDisable()
	{
		bCount = false;
	}
}
