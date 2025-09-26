using UnityEngine;

public class UIFishing_Catalog_Page : MonoBehaviour
{
	public TUIMeshSprite fishIcon;

	public TUILabel fishName;

	public GameObject[] starObj;

	public TUILabel quantity;

	public TUILabel weightest;

	public TUILabel page;

	private void Start()
	{
	}

	public void SetInfo(COMA_FishCatalog.Catalog page)
	{
		SetInfo(page.kind, page.num, page.maxWeight, page.star);
	}

	public void SetInfo(int kind, int num, int maxWeight, byte star)
	{
		page.Text = kind + 1 + "/" + COMA_FishCatalog.Instance.GetFishCount();
		if (num <= 0)
		{
			fishIcon.texture = "moshi6";
			fishIcon.flipX = false;
			fishIcon.transform.localScale = new Vector3(1f, 1f, 1f);
			fishName.TextID = string.Empty;
			fishName.Text = "???";
			starObj[0].SetActive(false);
			starObj[1].SetActive(false);
			starObj[2].SetActive(false);
			quantity.Text = "0";
			weightest.Text = "0KG";
			return;
		}
		fishIcon.texture = "fish_" + (kind + 1).ToString("d2");
		fishIcon.flipX = true;
		fishIcon.transform.localScale = new Vector3(6f, 6f, 6f);
		Fish_Param fishParam = COMA_Fishing_FishPool.Instance.GetFishParam(kind + 1);
		if (fishParam != null)
		{
			fishName.TextID = fishParam._strNameID;
		}
		else
		{
			fishName.TextID = string.Empty;
			fishName.Text = "???";
		}
		Debug.Log(star);
		if ((star & 4) == 0)
		{
			starObj[0].SetActive(false);
		}
		else
		{
			starObj[0].SetActive(true);
		}
		if ((star & 2) == 0)
		{
			starObj[1].SetActive(false);
		}
		else
		{
			starObj[1].SetActive(true);
		}
		if ((star & 1) == 0)
		{
			starObj[2].SetActive(false);
		}
		else
		{
			starObj[2].SetActive(true);
		}
		quantity.Text = num.ToString();
		weightest.Text = ((float)maxWeight / 1000f).ToString("f2") + "KG";
	}
}
