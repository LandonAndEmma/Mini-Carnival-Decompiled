using UnityEngine;

public class UIRPG_Map_GetMobilityMgr : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	[SerializeField]
	private UILabel _desLabel;

	public void OnEnable()
	{
		_vertexMgr.ScreenMoveScale.OnDisable();
		int energyValue_Max = RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max;
		uint num = (uint)(RPGGlobalData.Instance.RpgMiscUnit._energyRenewTimePerUnit * energyValue_Max * 60);
		int energyRenewPricePerMinute = RPGGlobalData.Instance.RpgMiscUnit._energyRenewPricePerMinute;
		uint num2 = RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32() - UIDataBufferCenter.Instance.RPGData.m_mobility_time;
		int num3 = Mathf.CeilToInt((float)(num - num2) / 60f);
		int num4 = energyRenewPricePerMinute * num3;
		Debug.Log("value = " + num4);
		_desLabel.text = TUITool.StringFormat(Localization.instance.Get("energy_desc1"), UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("00aaff", num4));
	}
}
