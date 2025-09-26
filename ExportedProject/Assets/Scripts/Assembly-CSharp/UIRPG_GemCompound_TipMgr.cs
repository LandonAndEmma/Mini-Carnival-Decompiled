using UnityEngine;

public class UIRPG_GemCompound_TipMgr : MonoBehaviour
{
	[SerializeField]
	private UIRPG_GemCompoundMgr _gemCompondMgr;

	[SerializeField]
	private GameObject _prefab;

	[SerializeField]
	private Transform _prefabParent;

	[SerializeField]
	private UIDraggablePanel _draggablePanel;

	[SerializeField]
	private UIPanel _containerPanel;

	public void Init()
	{
		int num = 0;
		foreach (int key in RPGGlobalData.Instance.CompoundTableUnitPool._dict.Keys)
		{
			if (key.ToString().Contains(((int)_gemCompondMgr.CurActiveCaption).ToString()))
			{
				GameObject gameObject = Object.Instantiate(_prefab) as GameObject;
				gameObject.transform.parent = _prefabParent;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				if (num < 10)
				{
					gameObject.name = "UIRPG_CardMgr_GemCompoundTip0" + num++;
				}
				else
				{
					gameObject.name = "UIRPG_CardMgr_GemCompoundTip" + num++;
				}
				UIRPG_GemCompound_TipBoxData boxData = new UIRPG_GemCompound_TipBoxData(key, _gemCompondMgr.CurLevel);
				gameObject.GetComponent<UIRPG_GemCompound_TipBox>().BoxData = boxData;
			}
		}
		_prefabParent.GetComponent<UIGrid>().Reposition();
		_containerPanel.gameObject.transform.localPosition = Vector3.zero;
		Vector4 clipRange = _containerPanel.clipRange;
		clipRange.x = 0f;
		clipRange.y = 0f;
		_containerPanel.clipRange = clipRange;
		if (num < 5)
		{
			_draggablePanel.scale = Vector3.zero;
		}
		else
		{
			_draggablePanel.scale = new Vector3(0f, 1f, 0f);
		}
	}

	public void OnEnable()
	{
		Init();
	}

	public void OnDisable()
	{
		for (int i = 0; i < _prefabParent.childCount; i++)
		{
			Object.Destroy(_prefabParent.GetChild(i).gameObject);
		}
		Debug.Log("OnDisable() -----------------------------");
	}
}
