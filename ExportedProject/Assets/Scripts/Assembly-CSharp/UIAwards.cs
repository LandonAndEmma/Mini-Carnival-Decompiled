using UnityEngine;

public class UIAwards : UIMessageHandler
{
	[SerializeField]
	private TUIScrollList_Avatar _scrollListAwards;

	[SerializeField]
	private TUIClipBinder clipBinderCmp;

	[SerializeField]
	private GameObject _oneAwardPrefab;

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_OK(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			ProcessGetAward(null);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void InitList(ref UI_OneAwardData[][] datas)
	{
		_scrollListAwards.Clear(true);
		for (int i = 0; i < datas.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(_oneAwardPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UI_OneAward component = gameObject.GetComponent<UI_OneAward>();
			component.OneAwardData = datas[i];
			Debug.Log("oneAward.OneAwardData=" + component.OneAwardData);
			TUIControl component2 = gameObject.GetComponent<TUIControl>();
			if (component2 == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			_scrollListAwards.Add(component2);
		}
		_scrollListAwards.ScrollListTo(0f);
		clipBinderCmp.SetClipRect();
	}

	public void ProcessGetAward(UI_OneAward oneAward)
	{
		int num = 0;
		for (int i = 0; i < COMA_Server_Award.Instance.lst_awards.Count; i++)
		{
			if (COMA_Server_Award.Instance.lst_awards[i].nAwardNum <= 0)
			{
				num++;
			}
		}
		for (int j = 0; j < COMA_Server_Award.Instance.lst_ranking_awards.Count; j++)
		{
			if (COMA_Server_Award.Instance.lst_ranking_awards[j].nAwardNum <= 0)
			{
				num++;
			}
		}
		if (num > COMA_Pref.Instance.PackageNullCount())
		{
			TUI_MsgBox.Instance.MessageBox(107);
			return;
		}
		COMA_Server_Award.Instance.GetAllRewards();
		UIMainMenu component = base.transform.root.GetComponent<UIMainMenu>();
		if (component != null)
		{
			component.AwardsNum = 0;
			component.RefreshGoldAndCrystal();
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		Object.Destroy(base.gameObject);
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			Object.Destroy(base.gameObject);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
