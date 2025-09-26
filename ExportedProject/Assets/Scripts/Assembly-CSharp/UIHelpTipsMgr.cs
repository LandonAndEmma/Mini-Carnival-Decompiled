using System.Collections.Generic;
using UnityEngine;

public class UIHelpTipsMgr : MonoBehaviour
{
	private bool bShow;

	[SerializeField]
	private UILabel _labelTips;

	private void Start()
	{
	}

	private void Update()
	{
		if (!bShow && COMA_DataConfig.Instance != null)
		{
			List<string> lstHelpTips = COMA_DataConfig.Instance._helpTips.LstHelpTips;
			if (lstHelpTips != null && lstHelpTips.Count > 0)
			{
				Debug.Log(lstHelpTips.Count);
				int num = Random.Range(0, lstHelpTips.Count);
				Debug.Log(num);
				string key = lstHelpTips[num];
				_labelTips.text = Localization.instance.Get(key);
				bShow = true;
			}
		}
	}
}
