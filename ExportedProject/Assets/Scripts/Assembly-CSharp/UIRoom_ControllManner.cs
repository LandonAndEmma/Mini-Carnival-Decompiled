using UnityEngine;

public class UIRoom_ControllManner : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _mannersSel;

	private void Start()
	{
		if (COMA_Pref.Instance.controlMode_Run == -1)
		{
			SetSelIndex(1);
			COMA_Pref.Instance.controlMode_Run = 1;
		}
		else
		{
			SetSelIndex(COMA_Pref.Instance.controlMode_Run);
		}
	}

	public void SetSelIndex(int n)
	{
		for (int i = 0; i < _mannersSel.Length; i++)
		{
			if (i == n)
			{
				_mannersSel[i].SetActive(true);
			}
			else
			{
				_mannersSel[i].SetActive(false);
			}
		}
	}

	private void Update()
	{
	}
}
