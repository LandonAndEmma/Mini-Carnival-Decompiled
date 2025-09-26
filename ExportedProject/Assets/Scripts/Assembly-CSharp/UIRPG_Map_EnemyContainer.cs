using System.Collections.Generic;
using NGUI_COMUI;

public class UIRPG_Map_EnemyContainer : NGUI_COMUI.UI_Container
{
	public override void DataSort()
	{
		List<NGUI_COMUI.UI_BoxData> list = new List<NGUI_COMUI.UI_BoxData>();
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_Map_EnemyBoxData uIRPG_Map_EnemyBoxData = base.LstBoxs[i].BoxData as UIRPG_Map_EnemyBoxData;
			list.Add(base.LstBoxs[i].BoxData);
		}
		list.Sort();
		for (int j = 0; j < list.Count; j++)
		{
			base.LstBoxs[j].BoxData = list[j];
		}
	}
}
