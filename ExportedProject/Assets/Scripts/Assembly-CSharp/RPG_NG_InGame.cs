using UnityEngine;

public class RPG_NG_InGame : MonoBehaviour
{
	private float _EnterTime;

	[SerializeField]
	private GameObject _obj;

	private void Start()
	{
		if (!RPGRefree.Instance.IsPCLoadFromCurScene())
		{
			_obj.SetActive(COMA_Pref.Instance != null && COMA_Pref.Instance.NG2_1_FirstEnterRPGGame);
			_EnterTime = Time.time;
			if (COMA_Pref.Instance.NG2_1_FirstEnterRPGGame)
			{
				COMA_Pref.Instance.NG2_1_FirstEnterRPGGame = false;
				COMA_Pref.Instance.Save(true);
			}
		}
	}

	private void Update()
	{
		if (Time.time - _EnterTime > 3f)
		{
			_obj.SetActive(false);
		}
	}
}
