using MC_UIToolKit;
using UnityEngine;

public class UIAutoDelBlockOnlyMessageBoxMgr : MonoBehaviour
{
	public static UIAutoDelBlockOnlyMessageBoxMgr Instance;

	private int nAutoCount;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	protected void OnEnable()
	{
		Instance = this;
	}

	protected void OnDisable()
	{
		Instance = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ReleaseAutoDelBlockOnlyMessageBox()
	{
		nAutoCount--;
		if (nAutoCount <= 0)
		{
			UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		}
	}

	public void PopAutoDelBlockOnlyMessageBox(int nCount)
	{
		nAutoCount = nCount;
		Debug.Log("PopAutoDelBlockOnlyMessageBox=" + nAutoCount);
		if (nAutoCount > 0)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
		}
	}
}
