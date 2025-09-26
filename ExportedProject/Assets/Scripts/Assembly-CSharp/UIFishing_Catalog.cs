using UnityEngine;

public class UIFishing_Catalog : MonoBehaviour
{
	public UIFishing_Catalog_Page page_left;

	public UIFishing_Catalog_Page page_left_move;

	public UIFishing_Catalog_Page page_right;

	public UIFishing_Catalog_Page page_right_move;

	private int curPage;

	private int pageCount;

	private bool bPageRight = true;

	private void Start()
	{
		Debug.Log("Fish Book Start!!!");
		pageCount = COMA_FishCatalog.Instance.GetFishCount();
		page_left.SetInfo(COMA_FishCatalog.Instance.lst[curPage]);
		page_right_move.SetInfo(COMA_FishCatalog.Instance.lst[curPage + 1]);
	}

	public void HandleEventButton_FlipLeft(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_FlipLeft " + curPage);
		if (curPage + 3 < pageCount)
		{
			page_left_move.SetInfo(COMA_FishCatalog.Instance.lst[curPage + 2]);
			page_right.SetInfo(COMA_FishCatalog.Instance.lst[curPage + 3]);
			curPage += 2;
			base.animation["UI_FishingCatalogFlip"].time = 0f;
			base.animation["UI_FishingCatalogFlip"].speed = 5f;
			base.animation.Play();
			if (!bPageRight)
			{
				page_left.SetInfo(COMA_FishCatalog.Instance.lst[curPage]);
			}
			bPageRight = false;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Page_Turn);
		}
	}

	public void HandleEventButton_FlipRight(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_FlipRight " + curPage);
		if (curPage - 2 >= 0)
		{
			page_right_move.SetInfo(COMA_FishCatalog.Instance.lst[curPage - 1]);
			page_left.SetInfo(COMA_FishCatalog.Instance.lst[curPage - 2]);
			curPage -= 2;
			base.animation["UI_FishingCatalogFlip"].time = base.animation["UI_FishingCatalogFlip"].length;
			base.animation["UI_FishingCatalogFlip"].speed = -5f;
			base.animation.Play();
			if (!bPageRight)
			{
				page_right.SetInfo(COMA_FishCatalog.Instance.lst[curPage + 1]);
			}
			bPageRight = true;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Page_Turn);
		}
	}

	public void HandleEventButton_Quit(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			base.gameObject.SetActive(false);
		}
	}
}
