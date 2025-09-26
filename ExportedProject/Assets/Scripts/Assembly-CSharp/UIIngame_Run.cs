using UnityEngine;

public class UIIngame_Run : MonoBehaviour
{
	[SerializeField]
	private GameObject[] mode0;

	[SerializeField]
	private GameObject[] mode1;

	[SerializeField]
	private GameObject[] mode2;

	private void Awake()
	{
		switch (COMA_Pref.Instance.controlMode_Run)
		{
		case 0:
		{
			for (int k = 0; k < mode0.Length; k++)
			{
				mode0[k].SetActive(true);
			}
			COMA_Run_SceneController.Instance.propsCom = mode0[0].transform.GetComponentInChildren<UIInGame_PropsBox>();
			COMA_Run_SceneController.Instance.InitPropIcons();
			break;
		}
		case 1:
		{
			for (int j = 0; j < mode1.Length; j++)
			{
				mode1[j].SetActive(true);
			}
			COMA_Run_SceneController.Instance.propsCom = mode1[0].transform.GetComponentInChildren<UIInGame_PropsBox>();
			COMA_Run_SceneController.Instance.InitPropIcons();
			break;
		}
		case 2:
		{
			for (int i = 0; i < mode2.Length; i++)
			{
				mode2[i].SetActive(true);
			}
			COMA_Run_SceneController.Instance.propsCom = mode2[0].transform.GetComponentInChildren<UIInGame_PropsBox>();
			COMA_Run_SceneController.Instance.InitPropIcons();
			break;
		}
		}
	}

	private void Update()
	{
	}
}
