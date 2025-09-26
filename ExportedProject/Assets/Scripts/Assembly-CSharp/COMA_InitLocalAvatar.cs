using UnityEngine;

public class COMA_InitLocalAvatar : MonoBehaviour
{
	public GameObject[] targetObj;

	public COMA_PlayerCharacter characterCom;

	private void Awake()
	{
	}

	private void Start()
	{
		if (targetObj == null || targetObj.Length < 3)
		{
			Debug.LogError("targetObj is Null!!");
		}
		UpdateAvatar();
	}

	public void UpdateAvatar()
	{
		for (int i = 0; i < targetObj.Length; i++)
		{
			targetObj[i].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[i]].texture;
		}
		characterCom.RemoveAllAccounterment();
		for (int j = 0; j < COMA_Pref.Instance.AInPack.Length; j++)
		{
			if (COMA_Pref.Instance.AInPack[j] >= 0)
			{
				characterCom.CreateAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[j]].serialName);
			}
		}
	}
}
