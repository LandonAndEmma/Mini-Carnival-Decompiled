using UnityEngine;

public class UISquare_ButtonKamcord : MonoBehaviour
{
	[SerializeField]
	private GameObject recordNumObj;

	private void OnEnable()
	{
		if (COMA_VideoController.Instance.nVideo != 0)
		{
			if (COMA_VideoController.Instance.bRecorded)
			{
				recordNumObj.SetActive(true);
			}
			else
			{
				recordNumObj.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (COMA_VideoController.Instance.bRecorded)
		{
			COMA_VideoController.Instance.bVideoPopView = true;
			COMA_VideoController.Instance.PlayLastRecording();
		}
		else
		{
			COMA_VideoController.Instance.bVideoPopView = true;
			COMA_VideoController.Instance.Show();
		}
		COMA_VideoController.Instance.bRecorded = false;
		COMA_VideoController.Instance.bClickKamcord = false;
		recordNumObj.SetActive(false);
	}
}
