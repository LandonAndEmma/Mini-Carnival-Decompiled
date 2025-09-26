using UnityEngine;

public class UI_ButtonScale : MonoBehaviour
{
	private Vector3[] childsScale;

	private void Awake()
	{
		int childCount = base.transform.childCount;
		childsScale = new Vector3[childCount];
		for (int i = 0; i < childCount; i++)
		{
			childsScale[i] = base.transform.GetChild(i).localScale;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BtnScale()
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Vector3 localScale = base.transform.GetChild(i).gameObject.transform.localScale;
			base.transform.GetChild(i).gameObject.transform.localScale = new Vector3(childsScale[i].x * 0.8f, childsScale[i].y * 0.8f, childsScale[i].z * 1f);
		}
	}

	public void BtnRestoreScale()
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			base.transform.GetChild(i).gameObject.transform.localScale = new Vector3(childsScale[i].x * 1f, childsScale[i].y * 1f, childsScale[i].z * 1f);
		}
	}
}
